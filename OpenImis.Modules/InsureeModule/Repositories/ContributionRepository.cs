using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.Modules.InsureeModule.Helpers;
using OpenImis.Modules.InsureeModule.Models;
using System;
using System.Data.SqlClient;

namespace OpenImis.Modules.InsureeModule.Repositories
{
    public class ContributionRepository : IContributionRepository
    {
        private IConfiguration Configuration;
        public int UserId { get; set; }

        public ContributionRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataMessage Enter(Contribution model)
        {
            DataHelper helper = new DataHelper(Configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@AuditUserID", UserId),
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
                var response = helper.Procedure("uspAPIEnterContribution", sqlParameters, 1);
                message = new EnterContibutionResponse(response.Code, false, response.Data, 0).Message;
            }
            catch (Exception e)
            {
                message = new EditFamilyResponse(e).Message;
            }

            return message;
        }
    }
}
