using OpenImis.Modules.LoginModule.Logic;

namespace OpenImis.Modules.LoginModule
{
    public interface ILoginModule
    {
        ILoginLogic GetLoginLogic();
        ILoginModule SetLoginLogic(ILoginLogic loginLogic);
    }
}
