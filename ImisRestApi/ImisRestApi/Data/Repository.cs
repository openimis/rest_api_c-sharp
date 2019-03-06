using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            DataTable dbUser = dh.FindUserByName(UserName);

            List<UserData> users = UserDataList(dbUser);

            if (users.Count == 1)
            {
                UserData user = users.FirstOrDefault();

                bool validUser = ValidateLogin(user,Password);

                if (validUser) {

                    List<string> userRights = dh.GetUserRights(user.UserID);
                    user.Rights = userRights;
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private bool ValidateLogin(UserData user,string password)
        {
            var generatedSHA = GenerateSHA256String(password + user.PrivateKey);
            if (generatedSHA == user.StoredPassword) {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenerateSHA256String(string inputString) {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();

            for(int i = 0; i < hash.Length;i++){
                var str = hash[i].ToString("X2");
                stringBuilder.Append(str);
            }

            return stringBuilder.ToString();
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
                    PrivateKey = Convert.ToString(rw["PrivateKey"]),
                    StoredPassword = Convert.ToString(rw["StoredPassword"])
                };

                list.Add(user);
            }
           
            return list;
        }

    }
}
