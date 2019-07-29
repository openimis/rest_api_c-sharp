using OpenImis.ModulesV1.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.LoginModule.Logic
{
    public interface ILoginLogic
    {
        UserData GetById(int userId);
        UserData FindUser(string UserName, string Password);
    }
}
