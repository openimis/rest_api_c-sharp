using OpenImis.Security.Models;
using System;

namespace OpenImis.Security.Logic
{
    public interface ILoginLogic
    {
        UserData GetByUUID(Guid userUUID);
        UserData FindUser(string UserName, string Password);
    }
}
