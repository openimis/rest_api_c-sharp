using OpenImis.Security.Logic;
using OpenImis.Security.Models;
using System;

namespace OpenImis.Security
{
    public interface ILoginModule
    {
        ILoginLogic GetLoginLogic();
        ILoginModule SetLoginLogic(ILoginLogic loginLogic);
    }
}
