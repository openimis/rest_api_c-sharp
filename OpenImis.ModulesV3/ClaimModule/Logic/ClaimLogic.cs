using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.ClaimModule.Models;
using OpenImis.ModulesV3.ClaimModule.Models.RegisterClaim;
using OpenImis.ModulesV3.ClaimModule.Repositories;
using System.Collections.Generic;

namespace OpenImis.ModulesV3.ClaimModule.Logic
{
    public class ClaimLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        protected ClaimRepository claimRepository;

        public ClaimLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            claimRepository = new ClaimRepository(_configuration, _hostingEnvironment);
        }

        public int Create(Claim claim)
        {
            int response;

            response = claimRepository.Create(claim);

            return response;
        }

        public DiagnosisServiceItem GetDsi(DsiInputModel model)
        {
            DiagnosisServiceItem message;

            message = claimRepository.GetDsi(model);

            return message;
        }

        public List<ClaimAdminModel> GetClaimAdministrators()
        {
            List<ClaimAdminModel> response;

            response = claimRepository.GetClaimAdministrators();

            return response;
        }

        public List<TblControls> GetControls()
        {
            List<TblControls> response;

            response = claimRepository.GetControls();

            return response;
        }

        public PaymentLists GetPaymentLists(PaymentListsInputModel model)
        {
            PaymentLists response;

            response = claimRepository.GetPaymentLists(model);

            return response;
        }

        public List<ClaimOutput> GetClaims(ClaimsModel model)
        {
            List<ClaimOutput> response;

            response = claimRepository.GetClaims(model);

            return response;
        }
    }
}
