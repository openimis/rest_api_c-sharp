using OpenImis.ModulesV2.LoginModule.Logic;

namespace OpenImis.ModulesV2.LoginModule
{
    public interface ILoginModule
    {
        ILoginLogic GetLoginLogic();
        ILoginModule SetLoginLogic(ILoginLogic loginLogic);
    }
}
