using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Data
{
    public class Repository
    {
        private DataHelper dh;

        public Repository(IConfiguration configuration)
        {
            dh = new DataHelper(configuration);
        }

        public UserData FindUser(string UserName, string Password)
        {

            DataTable dbUser = dh.Login(UserName, Password);

            List<UserData> users = UserDataList(dbUser);

            if (users.Count == 1)
            {
                UserData user = users.FirstOrDefault();

                
                return user;
            }
            else
            {
                return null;
            }
        }

        private List<UserData> UserDataList(DataTable dt)
        {
            List<UserData> list = new List<UserData>();
            
            for(int i = 0;i < dt.Rows.Count; i++)
            {
                var rw = dt.Rows[i];

                UserData user = new UserData()
                {
                    UserID = Convert.ToString(rw["UserID"]),
                    LoginName = Convert.ToString(rw["LoginName"]),
                    PrivateKey = Convert.ToString(rw["PrivateKey"])
                };

                list.Add(user);
            }
           
            return list;
        }

    }
}
