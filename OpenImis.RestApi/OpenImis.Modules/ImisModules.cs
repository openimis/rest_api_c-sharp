//using OpenImis.RestApi.Models.Interfaces;
using OpenImis.Modules;
using OpenImis.Modules.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules
{
    /// <summary>
    /// This Service instanciates the modules that will be used in the application entry point 
    /// </summary>
    public class ImisModules: IImisModules
    {
        private readonly IUserModule _userModule;
        public ImisModules()
        {
			_userModule = new UserModule.UserModule();
        }

        /// <summary>
        /// Return the user repository
        /// </summary>
        /// <returns>
        /// The instance of the user repository
        /// </returns>
        public IUserModule getUserModule()
        {
            return _userModule;
        }
    }
}
