using OpenImis.Modules.UserModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules
{
    /// <summary>
    /// This interface serves to define a Service for the Entities repositories 
    /// </summary>
    public interface IImisModules
    {
        /// <summary>
        /// Return the user repository
        /// </summary>
        /// <returns>
        /// The instance of the user repository
        /// </returns>
        IUserModule getUserModule();
    }
}
