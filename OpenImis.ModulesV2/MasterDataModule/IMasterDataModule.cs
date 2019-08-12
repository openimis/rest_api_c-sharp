using OpenImis.ModulesV2.MasterDataModule.Logic;

namespace OpenImis.ModulesV2.MasterDataModule
{
    public interface IMasterDataModule
    {
        IMasterDataLogic GetMasterDataLogic();
        IMasterDataModule SetMasterDataLogic(IMasterDataLogic masterDataLogic);
    }
}
