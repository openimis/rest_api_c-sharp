using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.Modules.ClaimModule.Models;
using OpenImis.Modules.ClaimModule.Models.RegisterClaim;
using OpenImis.Modules.ClaimModule.Repositories;
using System.Collections.Generic;

namespace OpenImis.Modules.ClaimModule.Logic
{
    public class ClaimLogic : IClaimLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        protected IClaimRepository claimRepository;

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
            List<ClaimAdminModel> response = new List<ClaimAdminModel>();

            response = claimRepository.GetClaimAdministrators();

            return response;
        }

        public List<TblControls> GetControls()
        {
            List<TblControls> response = new List<TblControls>();

            response = claimRepository.GetControls();

            return response;
        }

        public PaymentLists GetPaymentLists(PaymentListsInputModel model)
        {
            PaymentLists response;

            response = claimRepository.GetPaymentLists(model);

            return response;
        }
    }
}
