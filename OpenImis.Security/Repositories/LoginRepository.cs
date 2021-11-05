using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.Security.Models;
using OpenImis.Security.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenImis.Security.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private IConfiguration Configuration;

        public LoginRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public UserData GetByUUID(Guid userUUID)
        {
            UserData user;
            using (var imisContext = new ImisDB())
            {
                user = imisContext.TblUsers.Where(u => u.UserUUID == userUUID).Select(x => new UserData()
                {
                    UserUUID = x.UserUUID,
                    LoginName = x.LoginName,
                    PrivateKey = x.PrivateKey,
                    StoredPassword = x.StoredPassword
                })
                .FirstOrDefault();
            }

            return user;
        }

        public List<UserData> FindUserByName(string UserName)
        {
            List<UserData> response = new List<UserData>();

            using (var imisContext = new ImisDB())
            {
                response = imisContext.TblUsers
                                .Where(u => u.LoginName == UserName && u.ValidityTo == null)
                                .Select(x => new UserData()
                                {
                                    UserUUID = x.UserUUID,
                                    LoginName = Convert.ToString(x.LoginName),
                                    PrivateKey = Convert.ToString(x.PrivateKey),
                                    StoredPassword = Convert.ToString(x.StoredPassword)
                                })
                                .ToList();
            }

            return response;
        }
    }
}
