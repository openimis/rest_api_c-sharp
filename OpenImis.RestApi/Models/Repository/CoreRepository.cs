using OpenImis.RestApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Models.Repository
{
    /// <summary>
    /// This Service instanciates the Entities repositories and serves as an entry point to these 
    /// </summary>
    public class CoreRepository: ICoreRepository
    {
        private readonly IUserRepository userRepository;
        public CoreRepository()
        {
            userRepository = new UserRepository();
        }

        /// <summary>
        /// Return the user repository
        /// </summary>
        /// <returns>
        /// The instance of the user repository
        /// </returns>
        public IUserRepository getUserRepository()
        {
            return userRepository;
        }
    }
}
