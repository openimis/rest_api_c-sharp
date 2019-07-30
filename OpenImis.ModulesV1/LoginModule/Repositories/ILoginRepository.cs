using OpenImis.ModulesV1.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.ModulesV1.LoginModule.Repositories
{
    public interface ILoginRepository
    {
        UserData GetById(int userId);
        List<UserData> FindUserByName(string UserName);
    }
}
