using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.ClaimModule.Logic;

namespace OpenImis.ModulesV2.ClaimModule
{
    public class ClaimModule : IClaimModule
    {
        private IConfiguration _configuration;

        private IClaimLogic _claimLogic;

        public ClaimModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IClaimLogic GetClaimLogic()
        {
            if (_claimLogic == null)
            {
                _claimLogic = new ClaimLogic(_configuration);
            }
            return _claimLogic;
        }

        public IClaimModule SetClaimLogic(IClaimLogic claimLogic)
        {
            _claimLogic = claimLogic;
            return this;
        }
    }
}
