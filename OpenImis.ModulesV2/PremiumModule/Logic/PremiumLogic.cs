using Microsoft.Extensions.Configuration;

namespace OpenImis.ModulesV2.PremiumModule.Logic
{
    public class PremiumLogic : IPremiumLogic
    {
        private IConfiguration _configuration;

        public PremiumLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
