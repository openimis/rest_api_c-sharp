using ImisRestApi.Models;
using ImisRestApi.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ImisRestApi.Data
{
    public class ImisValidate
    {
        private IConfiguration Configuration;

        public ImisValidate(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       

        public List<string> GetUserRights(string userId)
        {

            DataHelper helper = new DataHelper(Configuration);

            var rights = new List<string>();

            var sSQL = @"SELECT DISTINCT tblRoleRight.RightID                     
					     FROM tblRole INNER JOIN
                         tblRoleRight ON tblRole.RoleID = tblRoleRight.RoleID AnD tblRoleRight.ValidityTo IS NULL
						 INNER JOIN
                         tblUserRole ON tblRole.RoleID = tblUserRole.RoleID AND tblUserRole.ValidityTo IS NULL
						 INNER JOIN
                         tblUsers ON tblUserRole.UserID = tblUsers.UserID AND tblUsers.ValidityTo IS NULL
                         WHERE tblUsers.UserID = @UserId AND tblRole.ValidityTo IS NULL";

            SqlParameter[] paramets = {
                new SqlParameter("@UserId", userId)
            };

            var dt = helper.GetDataTable(sSQL, paramets, CommandType.Text);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var row = dt.Rows[i];
                    var rightId = Convert.ToInt32(row["RightID"]);
                    var rightName = Enum.GetName(typeof(Rights), rightId);

                    if (rightName != null)
                    {
                        rights.Add(rightName);
                    }

                }
            }

            return rights;
        }

        public DataTable FindUserByName(string UserName)
        {
            DataHelper helper = new DataHelper(Configuration);

            var sSQL = @"
                        SELECT UserID, UserUUID, LoginName, LanguageID, RoleID,StoredPassword,PrivateKey
                        FROM tblUsers
                        WHERE LoginName = @LoginName                        
                        AND ValidityTo is null
                        ";

            SqlParameter[] paramets = {
                new SqlParameter("@LoginName", UserName)
            };

            //var data = new DataHelper(Configuration);

            var dt = helper.GetDataTable(sSQL, paramets, CommandType.Text);

            return dt;
        }

        public UserData FindUser(string UserName, string Password)
        {

            DataTable dbUser = FindUserByName(UserName);

            List<UserData> users = UserDataList(dbUser);

            if (users.Count == 1)
            {
                UserData user = users.FirstOrDefault();

                bool validUser = ValidateLogin(user, Password);

                if (validUser)
                {

                    List<string> userRights = GetUserRights(user.UserID);
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

        private bool ValidateLogin(UserData user, string password)
        {
            var generatedSHA = GenerateSHA256String(password + user.PrivateKey);
            if (generatedSHA == user.StoredPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                var str = hash[i].ToString("X2");
                stringBuilder.Append(str);
            }

            return stringBuilder.ToString();
        }

        private List<UserData> UserDataList(DataTable dt)
        {
            List<UserData> list = new List<UserData>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var rw = dt.Rows[i];
                UserData user = new UserData()
                {
                    UserID = Convert.ToString(rw["UserID"]),
                    UserUUID = (Guid)rw["UserUUID"],
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