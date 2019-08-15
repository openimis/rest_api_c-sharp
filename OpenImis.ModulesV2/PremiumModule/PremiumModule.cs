using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.PremiumModule.Logic;

namespace OpenImis.ModulesV2.PremiumModule
{
    public class PremiumModule : IPremiumModule
    {
        private IConfiguration _configuration;
        private IPremiumLogic _premiumLogic;

        public PremiumModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IPremiumLogic GetPremiumLogic()
        {
            if (_premiumLogic == null)
            {
                _premiumLogic = new PremiumLogic(_configuration);
            }
            return _premiumLogic;
        }

        public IPremiumModule SetPremiumLogic(IPremiumLogic premiumLogic)
        {
            _premiumLogic = premiumLogic;
            return this;
        }
    }
}
