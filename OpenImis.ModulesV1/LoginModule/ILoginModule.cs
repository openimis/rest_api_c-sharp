using OpenImis.ModulesV1.LoginModule.Logic;


namespace OpenImis.ModulesV1.LoginModule
{
    public interface ILoginModule
    {
        ILoginLogic GetLoginLogic();
        ILoginModule SetLoginLogic(ILoginLogic loginLogic);
    }
}
