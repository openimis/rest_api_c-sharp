using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.Modules.LoginModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.LoginModule.Repositories
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
                    var rightName = Enum.GetName(typeof(Models.Rights), rightId);

                    if (rightName != null)
                    {
                        rights.Add(rightName);
                    }

                }
            }

            return rights;
        }
    }
}
