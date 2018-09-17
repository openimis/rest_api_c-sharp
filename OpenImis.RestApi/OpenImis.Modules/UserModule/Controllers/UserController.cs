using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.Modules.UserModule.Repository;
using OpenImis.Modules.UserModule.Entities;

namespace OpenImis.Modules.UserModule.Controllers
{
    /// <summary>
    /// This class is actual implementation of IUserRepository methods for Tanzania implementation 
    /// </summary>
    public class UserController: IUserController
    {

		private readonly IUserRepository _userRepository;

        public UserController() 
        {
			_userRepository = new UserSqlServer(); 
        }
				
		/// <summary>
		/// Get user by username
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public User GetByUsername(string username)
		{
			User user;

			user = _userRepository.GetByUsername(username);

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

			user = await _userRepository.GetByUsernameAndPasswordAsync(username, password);

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

			user = _userRepository.GetByUsernameAndPassword(username, password);

			return user;
		}

	}
}
