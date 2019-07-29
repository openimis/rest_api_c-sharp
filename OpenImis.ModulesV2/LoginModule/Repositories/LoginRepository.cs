using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenImis.ModulesV2.LoginModule.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private IConfiguration Configuration;

        public LoginRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public UserData GetById(int userId)
        {
            UserData user;
            using (var imisContext = new ImisDB())
            {
                user = imisContext.TblUsers.Where(u => u.UserId == userId).Select(x => new UserData()
                {
                    UserID = x.UserId.ToString(),
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

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblUsers
                                    .Where(u => u.LoginName == UserName && u.ValidityTo == null)
                                    .Select(x => new UserData()
                                    {
                                        UserID = Convert.ToString(x.UserId),
                                        LoginName = Convert.ToString(x.LoginName),
                                        PrivateKey = Convert.ToString(x.PrivateKey),
                                        StoredPassword = Convert.ToString(x.StoredPassword)
                                    })
                                    .ToList();
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
