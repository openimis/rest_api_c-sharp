using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.ModulesV1.InsureeModule.Helpers;
using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OpenImis.ModulesV1.InsureeModule.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private IConfiguration Configuration;

        public int UserId { get; set; }

        public PolicyRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataMessage Enter(Policy model)
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

        public DataMessage Renew(Policy model)
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
                var response = helper.GetDataTable("uspAPIRenewPolicy", sqlParameters, System.Data.CommandType.StoredProcedure);
                message = new RenewPolicyResponse(0, false, 0).Message;
            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;
        }

        public DataMessage GetCommissions(GetCommissionInputs model)
        {
            dynamic response;

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

            DataMessage message;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var res = (from PR in imisContext.TblPremium
                               join P in imisContext.TblPolicy.Where(p => p.ValidityTo == null) on PR.PolicyId equals P.PolicyId
                               join R in imisContext.TblReporting on PR.ReportingCommissionID equals R.ReportingId
                               join O in imisContext.TblOfficer on P.OfficerId equals O.OfficerId
                               where (PR.ReportingCommissionID == null
                                    && (PR.PayDate >= minDate && PR.PayDate <= maxDate)
                                    && PR.ValidityTo == null
                                    && O.Code == model.enrolment_officer_code)
                               select new
                               {
                                    Commission = (R.CammissionRate == null ? 0.00M : R.CammissionRate) * PR.Amount,
                                    PR.Amount
                               })
                               .ToList();

                    var c = res.Count > 0 ? res.Sum(x => x.Commission) : null;
                    var a = res.Count > 0 ? res.Sum(x => (decimal?)x.Amount) : null;

                    response = new List<dynamic>(){ new { Commission = c, Amount = a } };

                    message = new GetCommissionResponse(0, false, response, 0).Message;
                }
            }
            catch (Exception e)
            {
                message = new GetCommissionResponse(e).Message;
            }

            return message;
        }
    }
}