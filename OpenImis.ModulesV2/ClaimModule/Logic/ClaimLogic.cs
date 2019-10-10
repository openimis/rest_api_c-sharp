using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim;
using OpenImis.ModulesV2.ClaimModule.Repositories;

namespace OpenImis.ModulesV2.ClaimModule.Logic
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
    }
}
