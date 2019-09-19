using ImisRestApi.Models;
using ImisRestApi.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Data
{
    public class ImisClaims
    {
        private IConfiguration Configuration;

        public ImisClaims(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DiagnosisServiceItem GetDsi(DsiInputModel model)
        {

            var sSQL = @"SELECT ICDCode AS code ,ICDName AS name FROM tblICDCodes WHERE ValidityFrom >= @LastUpdated AND ValidityTo IS NULL
                         SELECT ItemCode AS code,ItemName As name,ItemPrice As price FROM tblItems WHERE ValidityFrom >= @LastUpdated AND ValidityTo IS NULL
                         SELECT ServCode as code,ServName AS name,ServPrice As price FROM tblServices WHERE ValidityFrom >= @LastUpdated AND ValidityTo IS NULL";

            DataHelper helper = new DataHelper(Configuration);
            SqlParameter date;

            if (model.last_update_date != null) {
                date  = new SqlParameter("@LastUpdated", model.last_update_date);
            }
            else
            {
                date = new SqlParameter("@LastUpdated", System.Data.SqlTypes.SqlDateTime.MinValue);
            }
            
            SqlParameter[] sqlParameters = {
                date,   
            };


            DiagnosisServiceItem message = new DiagnosisServiceItem();

            try
            {
                var response = helper.FillDataSet(sSQL, sqlParameters,CommandType.Text);
                if(response.Tables.Count > 0)
                {
                    var diagnoses = JsonConvert.SerializeObject(response.Tables[0]);
                    var items = JsonConvert.SerializeObject(response.Tables[1]);
                    var services = JsonConvert.SerializeObject(response.Tables[2]);

                    message.diagnoses = JsonConvert.DeserializeObject<List<CodeName>>(diagnoses);
                    message.items = JsonConvert.DeserializeObject<List<CodeNamePrice>>(items);
                    message.services = JsonConvert.DeserializeObject<List<CodeNamePrice>>(services);
                    message.update_since_last = DateTime.UtcNow;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return message;

        }

        public PaymentLists GetPaymentLists(PaymentListsInputModel model)
        {
            var sSQL = @"DECLARE @HFID INT, @PLServiceID INT, @PLItemID INT
                SELECT @HFID = HFId FROM tblClaimAdmin WHERE ClaimAdminCode = @ClaimAdminCode AND ValidityTo IS NULL 
                SELECT @PLServiceID = PLServiceID FROM tblHF Where HFid = @HFID 
                SELECT @PLItemID = PLItemID FROM tblHF Where HFid = @HFID


                SELECT tblHF.HFCode, tblHF.HFName FROM tblHF WHERE HfID = @HFID

                SELECT Items.ItemCode AS code, Items.ItemName AS name, ISNULL(PLID.PriceOverule,Items.ItemPrice) AS price from tblPLItemsDetail PLID
                INNER JOIN tblItems Items ON PLID.ItemID = Items.ItemID AND Items.ValidityTo IS NULL
                WHERE PLItemID = @PLItemID AND PLID.ValidityTo IS NULL AND (PLID.ValidityFrom >= @LastUpdated OR @LastUpdated IS NULL)                        

                Select SE.ServCode AS code, SE.ServName AS name, ISNULL(PLSD.PriceOverule,SE.ServPrice) AS price 
                FROM tblPLServicesDetail PLSD
                INNER JOIN tblServices SE ON PLSD.ServiceID = SE.ServiceID AND SE.ValidityTo IS NULL
                WHERE PLServiceID = @PLServiceID AND PLSD.ValidityTo IS NULL AND (PLSD.ValidityFrom >= @LastUpdated OR @LastUpdated IS NULL)    ";

            DataHelper helper = new DataHelper(Configuration);
            SqlParameter date;

            if (model.last_update_date != null)
            {
                date = new SqlParameter("@LastUpdated", model.last_update_date);
            }
            else
            {
                date = new SqlParameter("@LastUpdated", System.Data.SqlTypes.SqlDateTime.MinValue);
            }

            SqlParameter[] sqlParameters = {
                new SqlParameter("@ClaimAdminCode", model.claim_administrator_code),
                date
            };


            PaymentLists message = new PaymentLists();

            try
            {
                var response = helper.FillDataSet(sSQL, sqlParameters, CommandType.Text);
                if (response.Tables.Count > 0)
                {
                    var diagnoses = response.Tables[0];
                    var items = JsonConvert.SerializeObject(response.Tables[1]);
                    var services = JsonConvert.SerializeObject(response.Tables[2]);

                    if (diagnoses.Rows.Count > 0) {
                        message.health_facility_code = diagnoses.Rows[0]["HFCode"].ToString();
                        message.health_facility_name = diagnoses.Rows[0]["HFName"].ToString();
                    }
                    
                    message.pricelist_items = JsonConvert.DeserializeObject<List<CodeNamePrice>>(items);
                    message.pricelist_services = JsonConvert.DeserializeObject<List<CodeNamePrice>>(services);
                    message.update_since_last = DateTime.UtcNow;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return message;
        }

        internal object GetClaims(ClaimsModel model)
        {
          
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@ClaimAdminCode", model.claim_administrator_code),
                new SqlParameter("@StartDate", model.visit_date_from),
                new SqlParameter("@EndDate",  model.visit_date_to),
                new SqlParameter("@DateProcessedFrom",  model.processed_date_from),
                new SqlParameter("@DateProcessedTo",  model.processed_date_to),
                new SqlParameter("@ClaimStatus",  model.status_claim),
            };

            try
            {
                var response = helper.Procedure("uspAPIGetClaimOverview", sqlParameters);

                var responseData = response.Data;

                var jsonString = JsonConvert.SerializeObject(responseData);

                var ObjectList = JsonConvert.DeserializeObject<List<ClaimOutPut>>(jsonString).Distinct(new ClaimEqualityComparer());
                var services = JsonConvert.DeserializeObject<List<ClaimServices>>(jsonString).Distinct(new ServiceEqualityComparer());
                var items = JsonConvert.DeserializeObject<List<ClaimItems>>(jsonString).Distinct(new ItemEqualityComparer());

                List<ClaimOutPut> admin_claims = new List<ClaimOutPut>();

                foreach(var obj in ObjectList)
                {
                    var obj_services = services.Where(x => x.claim_number == obj.claim_number).ToList();
                    obj.services = obj_services;

                    var obj_items = items.Where(x => x.claim_number == obj.claim_number).ToList();
                    obj.items = obj_items;

                    admin_claims.Add(obj);
                }
                //var uniqueClaims = ObjectList.Distinct(new ClaimEqualityComparer());

                return admin_claims;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public DataTable GetClaimAdministrators()
        {
            var sSQL = @"SELECT LastName,OtherNames,ClaimAdminCode,HFCode FROM tblClaimAdmin CA
                         INNER JOIN tblHF H ON CA.HFID = H.HfID WHERE  H.ValidityTo IS NULL AND CA.ValidityTo IS NULL";

            DataHelper helper = new DataHelper(Configuration);
           
            SqlParameter[] sqlParameters = {
               
            };

            try
            {
                var response = helper.GetDataTable(sSQL, sqlParameters, CommandType.Text);
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }

            
        }

        public DataTable GetControls()
        {
            var sSQL = @"SELECT * FROM tblControls";

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
              
            };


            try
            {
                var response = helper.GetDataTable(sSQL, sqlParameters, CommandType.Text);

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


    }

    class ClaimEqualityComparer : IEqualityComparer<ClaimOutPut>
    {
        public bool Equals(ClaimOutPut x, ClaimOutPut y)
        {
            // Two items are equal if their keys are equal.
            return x.claim_number == y.claim_number;
        }

        public int GetHashCode(ClaimOutPut obj)
        {
            return obj.claim_number.GetHashCode();
        }
    }

    public class ServiceEqualityComparer : IEqualityComparer<ClaimServices>
    {
        public bool Equals(ClaimServices x, ClaimServices y)
        {
            // Two items are equal if their keys are equal.
            return x.service == y.service;
        }

        public int GetHashCode(ClaimServices obj)
        {
            
            if (obj.service != null)
            {
                return obj.service.GetHashCode();
            }
            else
            {
                return string.Empty.GetHashCode();
            }
        }
    }

    public class ItemEqualityComparer : IEqualityComparer<ClaimItems>
    {
        public bool Equals(ClaimItems x, ClaimItems y)
        {
            // Two items are equal if their keys are equal.
            return x.item == y.item;
        }

        public int GetHashCode(ClaimItems obj)
        {
            if(obj.item != null)
            {
                return obj.item.GetHashCode();
            }
            else
            {
                return string.Empty.GetHashCode();
            }
            
        }
    }
}
