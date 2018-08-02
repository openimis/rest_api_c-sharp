using OpenImis.RestApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.Modules.UserModule.Entities;
using Newtonsoft.Json;
using OpenImis.Modules.Utils;

namespace OpenImis.Modules.UserModule.Repository
{
    /// <summary>
    /// This class is actual implementation of IUserRepository methods for Tanzania implementation 
    /// </summary>
    public class UserSqlServer: IUserRepository
	{

        public UserSqlServer()
        {
        }

        public User GetById(int userId)
        {
            return new User();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            TblUsers user;
            using (var imisContext = new IMISContext())
            {
                user = await imisContext.TblUsers.Where(u => u.LoginName == username).FirstOrDefaultAsync();
            }
            return TypeCast.Cast<User>(user); 
        }

        public User GetByUsername(string username)
        {
            User user;
            using (var imisContext = new IMISContext())
            {
                user = (User)imisContext.TblUsers.Where(u => u.LoginName == username).FirstOrDefault();
            }
            return user;
        }

        /// <summary>
        /// Creates the SQL string for the GetByUsernameAndPassword[Async] calls
        /// </summary>
        /// <param name="userParameter"></param>
        /// <param name="passwordParameter"></param>
        /// <returns></returns>
        private RawSqlString GetByUsernameAndPasswordSQL() {
            return new RawSqlString($@"OPEN SYMMETRIC KEY EncryptionKey DECRYPTION BY Certificate EncryptData
                        SELECT *
                        FROM TblUsers
                        WHERE LoginName = @user
                        AND CONVERT(NVARCHAR(25), DECRYPTBYKEY(Password)) COLLATE LATIN1_GENERAL_CS_AS = @password
                        AND ValidityTo is null
                        CLOSE SYMMETRIC KEY EncryptionKey");
        }

        /// <summary>
        /// Get user by username and password by asychronious call
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            TblUsers user;
            using (var imisContext = new IMISContext())
            {
                var userParameter = new SqlParameter("user", username);
                var passwordParameter = new SqlParameter("password", password);

                user = await imisContext.TblUsers.FromSql(GetByUsernameAndPasswordSQL(),userParameter, passwordParameter).SingleOrDefaultAsync();
            }
            return TypeCast.Cast<User>(user);
        }

		
        /// <summary>
        /// Get user by username and password by sychronious call
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetByUsernameAndPassword(string username, string password)
        {
            User user;
            using (var imisContext = new IMISContext())
            {
                var userParameter = new SqlParameter("user", username);
                var passwordParameter = new SqlParameter("password", password);

                user = (User)imisContext.TblUsers.FromSql(GetByUsernameAndPasswordSQL(), userParameter, passwordParameter).SingleOrDefault();
            }
            return user;
        }

    }
}
