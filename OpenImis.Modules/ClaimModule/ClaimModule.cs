using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules.ClaimModule.Logic;

namespace OpenImis.Modules.ClaimModule
{
    public class ClaimModule : IClaimModule
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        private IClaimLogic _claimLogic;

        public ClaimModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IClaimLogic GetClaimLogic()
        {
            if (_claimLogic == null)
            {
                _claimLogic = new ClaimLogic(_configuration, _hostingEnvironment);
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
