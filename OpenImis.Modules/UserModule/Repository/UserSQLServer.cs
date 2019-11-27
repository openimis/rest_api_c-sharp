using OpenImis.DB.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
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
            using (var imisContext = new ImisDB())
            {
                user = await imisContext.TblUsers.Where(u => u.LoginName == username).FirstOrDefaultAsync();
            }
            return TypeCast.Cast<User>(user); 
        }

        public User GetByUsername(string username)
        {
			TblUsers user;
            using (var imisContext = new ImisDB())
            {
                user = imisContext.TblUsers.Where(u => u.LoginName == username).FirstOrDefault();
            }
            return TypeCast.Cast<User>(user);
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
            using (var imisContext = new ImisDB())
            {
                user = await imisContext.TblUsers.Where(u => u.LoginName == username).FirstOrDefaultAsync();

                if (user != null)
                {
                    if (!ValidateLogin(user.StoredPassword, user.PrivateKey, password))
                    {
                        user = null;
                    }
                }
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
            User user = GetByUsername(username);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (ValidateLogin(user.StoredPassword, user.PrivateKey, password))
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        private bool ValidateLogin(string storedPassword, string privateKey, string password)
        {
            var generatedSHA = GenerateSHA256String(password + privateKey);
            if (generatedSHA == storedPassword)
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

    }
}
