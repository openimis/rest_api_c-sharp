using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ImisRestApi.Responses;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Data
{
    public class ImisPolicy
    {
        private IConfiguration Configuration;
        public int UserId { get; set; }

        public ImisPolicy(IConfiguration configuration)
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
                var response = helper.GetDataTable("uspAPIRenewPolicy", sqlParameters,System.Data.CommandType.StoredProcedure);
                message = new RenewPolicyResponse(0,false,0).Message;

            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;

        }
    }
}