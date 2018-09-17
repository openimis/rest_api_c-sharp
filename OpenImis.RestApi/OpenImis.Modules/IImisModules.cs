using OpenImis.Modules.UserModule;
using OpenImis.Modules.WSModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules
{
    /// <summary>
    /// This interface serves to define a Service for the IMIS modules 
    /// </summary>
    public interface IImisModules
    {
        /// <summary>
        /// Return the user module. The method instantiates the User Modules and sets the controllers based on the configuration file. 
		/// If no configuration file is provided or if the configuration string is missing, then the default UserController is used. 
        /// </summary>
        /// <returns>
        /// The instance of the user module
        /// </returns>
        IUserModule GetUserModule();

        IWSModule GetWSModule();
    }
}
