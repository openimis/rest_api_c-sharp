using OpenImis.Security.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.Security.Repositories
{
    public interface ILoginRepository
    {
        UserData GetByUUID(Guid userUUID);
        List<UserData> FindUserByName(string UserName);
    }
}
