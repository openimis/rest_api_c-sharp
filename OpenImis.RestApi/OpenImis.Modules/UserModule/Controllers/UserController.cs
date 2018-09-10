using OpenImis.RestApi.Models.Entities;
//using OpenImis.RestApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.Modules.UserModule.Repository;
using OpenImis.Modules.UserModule.Entities;

namespace OpenImis.Modules.UserModule.Controller
{
    /// <summary>
    /// This class is actual implementation of IUserRepository methods for Tanzania implementation 
    /// </summary>
    public class UserController: IUserController
    {

		private readonly IUserModuleRepositories _sqlConfiguration;

        public UserController(IUserModuleRepositories sqlConfiguration) 
        {
			_sqlConfiguration = sqlConfiguration;
        }
		
		/// <summary>
		/// Get user by username
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public User GetByUsername(string username)
		{
			User user;

			user = _sqlConfiguration.GetUserSql().GetByUsername(username);

			return user;
		}

		/// <summary>
		/// Get user by username and password by asychronious call
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            User user;

			user = await _sqlConfiguration.GetUserSql().GetByUsernameAndPasswordAsync(username, password);

			return user;
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

			user = _sqlConfiguration.GetUserSql().GetByUsernameAndPassword(username, password);

			return user;
		}

	}
}
