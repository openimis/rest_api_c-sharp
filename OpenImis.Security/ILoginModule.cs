using OpenImis.Security.Logic;

namespace OpenImis.Security
{
    public interface ILoginModule
    {
        ILoginLogic GetLoginLogic();
        ILoginModule SetLoginLogic(ILoginLogic loginLogic);
    }
}
