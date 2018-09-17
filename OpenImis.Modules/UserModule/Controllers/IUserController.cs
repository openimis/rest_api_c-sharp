using OpenImis.Modules.UserModule.Entities;
using OpenImis.Modules.UserModule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule.Controllers
{
    /// <summary>
    /// This interface serves to define the methods which must be implemented in any specific instance 
    /// </summary>
    public interface IUserController
    {

		/// <summary>
		/// Get user by username
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		User GetByUsername(string username);


		/// <summary>
		/// Get user by username and password
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		Task<User> GetByUsernameAndPasswordAsync(string username, string password);

        /// <summary>
        /// Get user by username and password by sychronious call
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User GetByUsernameAndPassword(string username, string password);


    }
}
