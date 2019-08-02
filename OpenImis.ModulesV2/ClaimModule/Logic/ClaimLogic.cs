using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim;
using OpenImis.ModulesV2.ClaimModule.Repositories;

namespace OpenImis.ModulesV2.ClaimModule.Logic
{
    public class ClaimLogic : IClaimLogic
    {
        private IConfiguration _configuration;
        protected IClaimRepository claimRepository;

        public ClaimLogic(IConfiguration configuration)
        {
            _configuration = configuration;
            claimRepository = new ClaimRepository(_configuration);
        }

        public int RegisterClaim(Claim claim)
        {
            int response;

            response = claimRepository.RegisterClaim(claim);

            return response;
        }
    }
}
