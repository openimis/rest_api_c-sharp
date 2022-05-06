using OpenImis.ePayment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using OpenImis.ePayment.Responses;
using Microsoft.Extensions.Configuration;

namespace OpenImis.ePayment.Data
{
    public class ImisPolicy
    {
        private IConfiguration Configuration;
        public int UserId { get; set; }

        public ImisPolicy(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataMessage Enter(USSDPolicy model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
                new SqlParameter("@InsuranceNumber", model.InsuranceNumber),
                new SqlParameter("@EnrollmentDate", model.Date),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@EnrollmentOfficerCode", model.EnrollmentOfficerCode)
            };

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterPolicy", sqlParameters);
                message = new EnterPolicyResponse(response.Code, false, response.Data, 0).Message;

            }
            catch (Exception e)
            {
                message = new EditFamilyResponse(e).Message;
            }

            return message;

        }

        public DataMessage Renew(USSDPolicy model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
                new SqlParameter("@InsuranceNumber", model.InsuranceNumber),
                new SqlParameter("@RenewalDate", model.Date),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@EnrollmentOfficerCode", model.EnrollmentOfficerCode)
            };

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIRenewPolicy", sqlParameters);
                message = new RenewPolicyResponse(response.Code, false, 0).Message;

            }
            catch (Exception e)
            {
                message = new EditFamilyResponse(e).Message;
            }

            return message;

        }

        public DataMessage GetCommissions(USSDGetCommissionInputs model)
        {

            var sSQL = @"SELECT ISNULL(SUM(ISNULL(R.CammissionRate, 0.00) * PR.Amount),0.00) AS Commission, ISNULL(SUM(ISNULL(PR.Amount,0.00)),0.00) AS Amount
                        FROM tblPremium AS PR INNER JOIN
                             tblPolicy AS P ON P.PolicyID = PR.PolicyID AND P.ValidityTo IS NULL INNER JOIN
                             tblReporting AS R ON R.ReportingId = PR.ReportingCommissionID INNER JOIN
                             tblOfficer O ON P.OfficerID = O.OfficerID
                        WHERE  (PR.ReportingCommissionID IS NOT NULL) AND (PR.PayDate BETWEEN @MinDate and @MaxDate) 
                                AND (PR.ValidityTo IS NULL) AND (O.Code = @EnrollmentOfficerCode)";

            int year = DateTime.UtcNow.Year;
            int month = DateTime.UtcNow.Month;

            try
            {
                year = Convert.ToInt32(model.year);
                month = Convert.ToInt32(model.month);
            }
            catch (Exception)
            {
                throw;
            }

            DateTime minDate = new DateTime(year, month, 1);
            DateTime maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@MaxDate", maxDate),
                new SqlParameter("@MinDate", minDate),
                new SqlParameter("@Mode", model.mode),
                new SqlParameter("@EnrollmentOfficerCode", (model.enrolment_officer_code != null)?model.enrolment_officer_code:(object)DBNull.Value),
                new SqlParameter("@Payer",(model.payer != null)? model.payer:(object)DBNull.Value),
                new SqlParameter("@ProductCode", (model.insurance_product_code != null)? model.insurance_product_code:(object)DBNull.Value)
            };

            DataMessage message;

            try
            {
                var response = helper.GetDataTable(sSQL, sqlParameters, System.Data.CommandType.Text);
                message = new GetCommissionResponse(0, false, response, 0).Message;
            }
            catch (Exception e)
            {
                message = new GetCommissionResponse(e).Message;
            }

            return message;
        }
    }
}