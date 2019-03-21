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
    }
}
