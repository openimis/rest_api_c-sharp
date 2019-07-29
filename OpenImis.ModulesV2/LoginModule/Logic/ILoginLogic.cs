using OpenImis.ModulesV2.LoginModule.Models;

namespace OpenImis.ModulesV2.LoginModule.Logic
{
    public interface ILoginLogic
    {
        UserData GetById(int userId);
        UserData FindUser(string UserName, string Password);
    }
}
