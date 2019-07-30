using OpenImis.ModulesV2.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OpenImis.ModulesV2.LoginModule.Repositories
{
    public interface ILoginRepository
    {
        UserData GetById(int userId);
        List<UserData> FindUserByName(string UserName);
    }
}
