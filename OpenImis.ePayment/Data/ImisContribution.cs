using OpenImis.ePayment.Escape.Sms;
using OpenImis.ePayment.Models;
using OpenImis.ePayment.Models.Sms;
using OpenImis.ePayment.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OpenImis.ePayment.Data
{/*
    public class ImisContribution
    {
        private IConfiguration Configuration;
        public int UserId { get; set; }

        public ImisContribution(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }

        public DataMessage Enter(Contribution model)
        {
            DataHelper helper = new DataHelper(Configuration);

            bool reactionType = false;

            if (model.ReactionType == ReactionType.Active)
                reactionType = true;

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
                new SqlParameter("@InsuranceNumber", model.InsuranceNumber),
                new SqlParameter("@Payer", model.Payer),
                new SqlParameter("@PaymentDate", model.PaymentDate),
                new SqlParameter("@ProductCode", model.ProductCode),
                new SqlParameter("@ReceiptNo", model.ReceiptNumber),
                new SqlParameter("@ReactionType", reactionType),
                new SqlParameter("@ContributionCategory", model.ContributionCategory),
                new SqlParameter("@ContributionAmount", model.Amount),
                new SqlParameter("@PaymentType", model.PaymentType)
            };
           
            DataMessage message;

            try
            {
                var response = helper.Procedure("uspAPIEnterContribution", sqlParameters,1);
                message = new EnterContibutionResponse(response.Code,false, response.Data, 0).Message;
                
            }
            catch (Exception e)
            {

                message = new EnterContibutionResponse(e).Message;
            }

            return message;

        }

        
    }*/
}