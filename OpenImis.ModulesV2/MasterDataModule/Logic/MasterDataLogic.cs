using Microsoft.Extensions.Configuration;

namespace OpenImis.ModulesV2.MasterDataModule.Logic
{
    public class MasterDataLogic : IMasterDataLogic
    {
        private IConfiguration _configuration;

        public MasterDataLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
