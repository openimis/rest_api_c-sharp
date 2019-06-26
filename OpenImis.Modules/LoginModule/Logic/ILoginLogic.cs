using OpenImis.Modules.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.LoginModule.Logic
{
    public interface ILoginLogic
    {
        UserData FindUser(string UserName, string Password);
    }
}
