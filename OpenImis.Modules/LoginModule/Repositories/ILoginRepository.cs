using OpenImis.Modules.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.Modules.LoginModule.Repositories
{
    public interface ILoginRepository
    {
        UserData GetByUUID(Guid userUUID);
        List<UserData> FindUserByName(string UserName);
    }
}
