using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV2.Utils
{
    public class Repository
    {
        public string GetLoginNameByUserUUID(Guid userUUID)
        {
            string response;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblUsers
                        .Where(u => u.UserUUID == userUUID)
                        .Select(x => x.LoginName)
                        .FirstOrDefault();
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
