using OpenImis.ModulesV3.MasterDataModule.Logic;

namespace OpenImis.ModulesV3.MasterDataModule
{
    public interface IMasterDataModule
    {
        IMasterDataLogic GetMasterDataLogic();
        IMasterDataModule SetMasterDataLogic(IMasterDataLogic masterDataLogic);
    }
}
