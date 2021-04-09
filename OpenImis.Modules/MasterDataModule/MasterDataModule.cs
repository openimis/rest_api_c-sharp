using Microsoft.Extensions.Configuration;
using OpenImis.Modules.MasterDataModule.Logic;

namespace OpenImis.Modules.MasterDataModule
{
    public class MasterDataModule : IMasterDataModule
    {
        private IConfiguration _configuration;
        private IMasterDataLogic _masterDataLogic;

        public MasterDataModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMasterDataLogic GetMasterDataLogic()
        {
            if (_masterDataLogic == null)
            {
                _masterDataLogic = new MasterDataLogic(_configuration);
            }
            return _masterDataLogic;
        }

        public IMasterDataModule SetMasterDataLogic(IMasterDataLogic masterDataLogic)
        {
            _masterDataLogic = masterDataLogic;
            return this;
        }
    }
}
