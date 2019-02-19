using ImisRestApi.Models;
using ImisRestApi.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        public DataMessage GetDsi(DsiInputModel model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@LastUpdated", model.last_update_date),
                
            };


            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterPolicy", sqlParameters);
                message = new Responses.GetDsiResponse(response, false).Message;

            }
            catch (Exception e)
            {
                message = new GetDsiResponse(e).Message;
            }

            return message;

        }

        internal object GetPaymentLists(PaymentListsInputModel model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@ClaimAdministratorCode", model.claim_administrator_code),
                new SqlParameter("@LastUpdated", model.last_update_date)
            };


            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterPolicy", sqlParameters);
                message = new GetPaymentListResponse(response, false).Message;

            }
            catch (Exception e)
            {
                message = new GetPaymentListResponse(e).Message;
            }

            return message;
        }
    }
}
