using OpenImis.Modules.LoginModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.LoginModule
{
    public interface ILoginModule
    {
        ILoginLogic GetLoginLogic();
    }
}
