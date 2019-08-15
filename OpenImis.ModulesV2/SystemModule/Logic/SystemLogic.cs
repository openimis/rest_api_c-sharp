using Microsoft.Extensions.Configuration;

namespace OpenImis.ModulesV2.SystemModule.Logic
{
    public class SystemLogic : ISystemLogic
    {
        private IConfiguration _configuration;

        public SystemLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
