using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
            string sSQL = @"exec uspAPIGetClaims @ClaimAdminCode,@StartDate,@EndDate,@DateProcessedFrom,@DateProcessedTo,@ClaimStatus";

            DataHelper helper = new DataHelper(Configuration);
            int? claimStatus = null;

            if (model.status_claim != 0)
                claimStatus = (int)model.status_claim;

            SqlParameter[] sqlParameters = {
                new SqlParameter("@ClaimAdminCode", model.claim_administrator_code),
                new SqlParameter("@StartDate",SqlDbType.DateTime){ Value = (model.visit_date_from != null)?model.visit_date_from:(object)DBNull.Value},
                new SqlParameter("@EndDate",SqlDbType.DateTime){ Value = (model.visit_date_to != null)?model.visit_date_to:(object)DBNull.Value},
                new SqlParameter("@DateProcessedFrom",SqlDbType.DateTime){ Value = (model.processed_date_from != null)?model.processed_date_from:(object)DBNull.Value},
                new SqlParameter("@DateProcessedTo", SqlDbType.DateTime){ Value = (model.processed_date_to != null)?model.processed_date_to:(object)DBNull.Value},
                new SqlParameter("@ClaimStatus", SqlDbType.Int){ Value = (claimStatus != null)?claimStatus:(object)DBNull.Value},
            };

            try
            {
                var response = helper.GetDataSet(sSQL, sqlParameters,CommandType.Text);

                DataTable responseItems = response.Tables[0];
                DataTable responseServices = response.Tables[1];
                DataTable responseClaims = response.Tables[2];

                Dictionary<string, ClaimOutPut> admin_claims = (from DataRow dr in responseClaims.Rows 
                                                                     select new ClaimOutPut()
                {
                    claim_uuid = dr["claim_uuid"].ToStringWithDBNull(),
                    health_facility_code = dr["health_facility_code"].ToStringWithDBNull(),
                    health_facility_name = dr["health_facility_name"].ToStringWithDBNull(),
                    insurance_number = dr["insurance_number"].ToStringWithDBNull(),
                    patient_name = dr["patient_name"].ToStringWithDBNull(),
                    main_dg = dr["main_dg"].ToStringWithDBNull(),
                    claim_number = dr["claim_number"].ToStringWithDBNull(),
                    date_claimed = dr["date_claimed"].ToStringWithDBNull(),
                    visit_date_from = dr["visit_date_from"].ToStringWithDBNull(),
                    visit_type = dr["visit_type"].ToStringWithDBNull(),
                    claim_status = dr["claim_status"].ToStringWithDBNull(),
                    sec_dg_1 = dr["sec_dg_1"].ToStringWithDBNull(),
                    sec_dg_2 = dr["sec_dg_2"].ToStringWithDBNull(),
                    sec_dg_3 = dr["sec_dg_3"].ToStringWithDBNull(),
                    sec_dg_4 = dr["sec_dg_4"].ToStringWithDBNull(),
                    visit_date_to = dr["visit_date_to"].ToStringWithDBNull(),
                    claimed = dr["claimed"].ToString().ParseNullableDecimal(),
                    approved = dr["approved"].ToString().ParseNullableDecimal(),
                    adjusted = dr["adjusted"].ToString().ParseNullableDecimal(),
                    explanation = dr["explanation"].ToStringWithDBNull(),
                    adjustment = dr["adjustment"].ToStringWithDBNull(),
                    guarantee_number = dr["guarantee_number"].ToStringWithDBNull(),
                    services = new List<ClaimService>(),
                    items = new List<ClaimItem>()
                }).ToList().ToDictionary(x => x.claim_uuid, x => x,StringComparer.OrdinalIgnoreCase);

                (from DataRow dr in responseItems.Rows
                 select new ClaimItem()
                 {
                     claim_uuid = dr["claim_uuid"].ToStringWithDBNull(),
                     claim_number = dr["claim_number"].ToStringWithDBNull(),
                     item = dr["item"].ToStringWithDBNull(),
                     item_code = dr["item_code"].ToStringWithDBNull(),
                     item_qty = dr["item_qty"].ToString().ParseNullableDecimal(),
                     item_price = dr["item_price"].ToString().ParseNullableDecimal(),
                     item_adjusted_qty = dr["item_adjusted_qty"].ToString().ParseNullableDecimal(),
                     item_adjusted_price = dr["item_adjusted_price"].ToString().ParseNullableDecimal(),
                     item_explination = dr["item_explination"].ToStringWithDBNull(),
                     item_justificaion = dr["item_justificaion"].ToStringWithDBNull(),
                     item_valuated = dr["item_valuated"].ToString().ParseNullableDecimal(),
                     item_result = dr["item_result"].ToStringWithDBNull(),
                 }).ToList().ForEach((x) => { if (admin_claims.ContainsKey(x.claim_uuid)) admin_claims[x.claim_uuid].items.Add(x); });

                (from DataRow dr in responseServices.Rows
                 select new ClaimService()
                 {
                     claim_uuid = dr["claim_uuid"].ToStringWithDBNull(),
                     claim_number = dr["claim_number"].ToStringWithDBNull(),
                     service = dr["service"].ToStringWithDBNull(),
                     service_code = dr["service_code"].ToStringWithDBNull(),
                     service_qty = dr["service_qty"].ToString().ParseNullableDecimal(),
                     service_price = dr["service_price"].ToString().ParseNullableDecimal(),
                     service_adjusted_qty = dr["service_adjusted_qty"].ToString().ParseNullableDecimal(),
                     service_adjusted_price = dr["service_adjusted_price"].ToString().ParseNullableDecimal(),
                     service_explination = dr["service_explination"].ToStringWithDBNull(),
                     service_justificaion = dr["service_justificaion"].ToStringWithDBNull(),
                     service_valuated = dr["service_valuated"].ToString().ParseNullableDecimal(),
                     service_result = dr["service_result"].ToStringWithDBNull(),
                 }).ToList().ForEach((x) => { if (admin_claims.ContainsKey(x.claim_uuid)) admin_claims[x.claim_uuid].services.Add(x); });

                List<ClaimOutPut> output = admin_claims.Values.ToList();

                return output;
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

    public class ServiceEqualityComparer : IEqualityComparer<ClaimService>
    {
        public bool Equals(ClaimService x, ClaimService y)
        {
            // Two items are equal if their keys are equal.
            return x.service_code == y.service_code && x.claim_number == y.claim_number;
        }

        public int GetHashCode(ClaimService obj)
        {
            
            if (obj.service_code != null && obj.claim_number != null)
            {
                return obj.service_code.GetHashCode() + obj.claim_number.GetHashCode();
            }
            else
            {
                return string.Empty.GetHashCode();
            }
        }
    }

    public class ItemEqualityComparer : IEqualityComparer<ClaimItem>
    {
        public bool Equals(ClaimItem x, ClaimItem y)
        {
            // Two items are equal if their keys are equal.
            return x.item_code == y.item_code && x.claim_number == y.claim_number;
        }

        public int GetHashCode(ClaimItem obj)
        {
            if(obj.item_code != null && obj.claim_number != null)
            {
                return obj.item_code.GetHashCode() + obj.claim_number.GetHashCode();
            }
            else
            {
                return string.Empty.GetHashCode();
            }
            
        }
    }
}
