using ImisRestApi.Models;
using ImisRestApi.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ImisRestApi.Data
{
    public class ImisContribution
    {
        private IConfiguration Configuration;

        public ImisContribution(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataMessage Enter(Contribution model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@InsuranceNumber", model.InsuranceNumber),
                new SqlParameter("@Payer", model.Payer),
                new SqlParameter("@PaymentDate", model.PaymentDate),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@ReceiptNo", model.ReceiptNumber),
                new SqlParameter("@ReactionType", model.ReactionType),
                new SqlParameter("@ContributionCategory", model.ContributionCategory),
                new SqlParameter("@ContributionAmount", model.Amount),
                new SqlParameter("@PaymentType", model.PaymentType)
            };

            
            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterContribution", sqlParameters);
                message = new EnterContibutionResponse(response,false).Message;

            }
            catch (Exception e)
            {

                message = new EditFamilyResponse(e).Message;
            }

            return message;

        }
    }
}