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
            var sSQL = @"SELECT tblHF.HFCode, tblHF.HFName
                         FROM tblClaimAdmin INNER JOIN
                         tblHF ON tblClaimAdmin.HFId = tblHF.HfID
                         WHERE tblHF.ValidityFrom >= @LastUpdated AND tblHF.ValidityTo IS NULL AND tblClaimAdmin.ValidityTo IS NULL AND tblClaimAdmin.ClaimAdminCode = @ClaimAdminCode

                         SELECT tblItems.ItemCode AS code, tblItems.ItemName AS name, tblItems.ItemPrice AS price
                         FROM tblClaim INNER JOIN
                         tblClaimItems ON tblClaim.ClaimID = tblClaimItems.ClaimID INNER JOIN
                         tblClaimAdmin ON tblClaim.ClaimAdminId = tblClaimAdmin.ClaimAdminId INNER JOIN
                         tblItems ON tblClaimItems.ItemID = tblItems.ItemID
                         WHERE tblClaimItems.ValidityFrom >= @LastUpdated AND tblClaimItems.ValidityTo IS NULL AND tblClaimAdmin.ValidityTo IS NULL AND tblClaimAdmin.ClaimAdminCode = @ClaimAdminCode

                         SELECT tblServices.ServCode AS code, tblServices.ServName AS name, tblServices.ServPrice AS price
                         FROM tblClaim INNER JOIN
                         tblClaimAdmin ON tblClaim.ClaimAdminId = tblClaimAdmin.ClaimAdminId INNER JOIN
                         tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID INNER JOIN
                         tblServices ON tblClaimServices.ServiceID = tblServices.ServiceID
                         WHERE tblClaimServices.ValidityFrom >= @LastUpdated AND tblClaimServices.ValidityTo IS NULL AND tblClaimAdmin.ValidityTo IS NULL AND tblClaimAdmin.ClaimAdminCode = @ClaimAdminCode";

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
