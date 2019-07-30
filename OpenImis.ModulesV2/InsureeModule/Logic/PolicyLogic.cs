using Microsoft.Extensions.Configuration;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public class PolicyLogic : IPolicyLogic
    {
        private IConfiguration _configuration;

        public PolicyLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
