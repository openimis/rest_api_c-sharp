using OpenImis.ePayment.Models;
using OpenImis.ePayment.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Data
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

            var sSQL = @";WITH TotalForItems AS
                        (
                            SELECT C.ClaimId, SUM(CI.PriceAsked * CI.QtyProvided)Claimed,
                            SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) Approved,
                            SUM(CI.PriceValuated)Adjusted,
                            SUM(CI.RemuneratedAmount)Remunerated
                            FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
                            WHERE C.ValidityTo IS NULL
                            AND CI.ValidityTo IS NULL
                            GROUP BY C.ClaimID
                        ), TotalForServices AS
                        (
                            SELECT C.ClaimId, SUM(CS.PriceAsked * CS.QtyProvided)Claimed,
                            SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) Approved,
                            SUM(CS.PriceValuated)Adjusted,
                            SUM(CS.RemuneratedAmount)Remunerated
                            FROM tblClaim C
                            LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
                            WHERE C.ValidityTo IS NULL
                            AND CS.ValidityTo IS NULL
                            GROUP BY C.ClaimID
                        )

                        SELECT ICD1.ICDName sec_dg_1, ICD.ICDName main_dg, C.DateProcessed, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode health_facility_code, HF.HFName health_facility_name, C.ClaimCode claim_number, CONVERT(NVARCHAR, C.DateClaimed, 111) date_claimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
	                    CONVERT(NVARCHAR, C.DateFrom, 111) visit_date_from, CONVERT(NVARCHAR, C.DateTo, 111) visit_date_to, Ins.CHFID insurance_number, Ins.LastName + ' ' + Ins.OtherNames patient_name,
	                    CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END claim_status,
                        C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) claimed, 
	                    COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) approved,
	                    COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) adjusted,
	                    COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated) paid,
	                    CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
                        CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode,
                        CASE WHEN CI.QtyProvided<> COALESCE(CI.QtyApproved, CI.QtyProvided) THEN I.ItemCode ELSE NULL END AdjustedItem,
	                    CASE WHEN CI.QtyProvided<> COALESCE(CI.QtyApproved, CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END item_qty,
	                    CASE WHEN CI.QtyProvided<> COALESCE(CI.QtyApproved , CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END item_adjusted_qty,
	                    CASE WHEN CS.QtyProvided<> COALESCE(CS.QtyApproved, CS.QtyProvided)  THEN S.ServCode ELSE NULL END AdjustedService,
	                    CASE WHEN CS.QtyProvided<> COALESCE(CS.QtyApproved, CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END service_qty,
	                    CASE WHEN CS.QtyProvided<> COALESCE(CS.QtyApproved , CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END service_adjusted_qty,
	                    C.Explanation explination,
                        CASE C.VisitType WHEN 'E' THEN 'Emergency' WHEN 'R' THEN 'Referral' WHEN 'O' THEN 'Others' END visit_type,
                      C.Adjustment adjustment,
                      C.GuaranteeId guarantee_number,
                      I.ItemName item, I.ItemCode item_code, CI.PriceAdjusted item_adjusted_price, I.ItemPrice item_price, CI.Explanation item_explination, CI.Justification item_justification, CI.PriceValuated item_valuated, CI.RejectionReason item_result,
                            S.ServName[service],S.ServCode service_code, CS.PriceAdjusted service_adjusted_price, S.ServPrice service_price, CS.Explanation service_explination, CS.Justification service_justification, CS.PriceValuated service_valuated, CI.RejectionReason item_result

                        FROM tblClaim C
                        LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID

                        LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID

                        LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID

                        LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID

                        LEFT OUTER JOIN tblHF HF ON C.HFID = HF.HfID

                        LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId

                        LEFT OUTER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId

                        LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID

                        LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId

                        LEFT OUTER JOIN tblICDCodes ICD ON C.ICDID = ICD.ICDID

                        LEFT OUTER JOIN tblICDCodes ICD1 ON C.ICDID1 = ICD1.ICDID

                        WHERE C.ValidityTo IS NULL
                        AND CA.ClaimAdminCode = @ClaimAdminCode

                        AND(C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)

                        AND ISNULL(C.DateTo, C.DateFrom) BETWEEN ISNULL(@StartDate, (SELECT CAST(-53690 AS DATETIME))) AND ISNULL(@EndDate, GETDATE())

                        AND(C.DateProcessed BETWEEN ISNULL(@DateProcessedFrom, CAST('1753-01-01' AS DATE)) AND ISNULL(@DateProcessedTo, GETDATE()) OR C.DateProcessed IS NULL)

                    ";

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
                var response = helper.GetDataTable(sSQL, sqlParameters,CommandType.Text);

                var responseData = response;

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
