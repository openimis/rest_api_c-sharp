using OpenImis.ModulesV2.LoginModule.Models;
using System;

namespace OpenImis.ModulesV2.LoginModule.Logic
{
    public interface ILoginLogic
    {
        UserData GetByUUID(Guid userUUID);
        UserData FindUser(string UserName, string Password);
    }
}
