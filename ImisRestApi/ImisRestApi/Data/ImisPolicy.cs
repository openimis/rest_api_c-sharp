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

        public ImisPolicy(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataMessage Enter(Policy model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsuranceNumber", model.InsuranceNumber),
                new SqlParameter("@EnrollmentDate", model.Date),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@EnrollmentOfficerCode", model.EnrollmentOfficerCode)
            };

            
            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterPolicy", sqlParameters);
                 message = new EnterPolicyResponse(response,false).Message;

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
                new SqlParameter("@InsuranceNumber", model.InsuranceNumber),
                new SqlParameter("@RenewalDate", model.Date),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@EnrollmentOfficerCode", model.EnrollmentOfficerCode)
            };

            

            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIRenewPolicy", sqlParameters);
                message = new RenewPolicyResponse(response,false).Message;

            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;

        }
    }
}