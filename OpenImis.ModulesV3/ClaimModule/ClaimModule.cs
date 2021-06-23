using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.ClaimModule.Logic;

namespace OpenImis.ModulesV3.ClaimModule
{
    public class ClaimModule 
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        private ClaimLogic _claimLogic;

        public ClaimModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public ClaimLogic GetClaimLogic()
        {
            if (_claimLogic == null)
            {
                _claimLogic = new ClaimLogic(_configuration, _hostingEnvironment);
            }
            return _claimLogic;
        }

        public ClaimModule SetClaimLogic(ClaimLogic claimLogic)
        {
            _claimLogic = claimLogic;
            return this;
        }
    }
}
