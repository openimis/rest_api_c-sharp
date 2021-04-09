using OpenImis.Modules.LoginModule.Models;
using System;

namespace OpenImis.Modules.LoginModule.Logic
{
    public interface ILoginLogic
    {
        UserData GetByUUID(Guid userUUID);
        UserData FindUser(string UserName, string Password);
    }
}
