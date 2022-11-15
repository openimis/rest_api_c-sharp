using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3.ClaimModule.Logic;

namespace OpenImis.ModulesV3.ClaimModule
{
    public class ClaimModule 
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;

        private ClaimLogic _claimLogic;

        public ClaimModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public ClaimLogic GetClaimLogic()
        {
            if (_claimLogic == null)
            {
                _claimLogic = new ClaimLogic(_configuration, _hostingEnvironment, _loggerFactory);
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
