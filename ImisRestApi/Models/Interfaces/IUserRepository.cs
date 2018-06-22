using ImisRestApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Interfaces
{
    /// <summary>
    /// This interface serves to define the methods which must be implemented in any specific instance 
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        TblUsers GetById(int userId);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        TblUsers GetByUsername(string username);

        /// <summary>
        /// Get user by username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        TblUsers GetByUsernameAndPassword(string username, string password);


    }
}
