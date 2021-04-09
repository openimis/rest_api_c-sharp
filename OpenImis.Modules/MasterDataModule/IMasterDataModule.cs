using OpenImis.Modules.MasterDataModule.Logic;

namespace OpenImis.Modules.MasterDataModule
{
    public interface IMasterDataModule
    {
        IMasterDataLogic GetMasterDataLogic();
        IMasterDataModule SetMasterDataLogic(IMasterDataLogic masterDataLogic);
    }
}
