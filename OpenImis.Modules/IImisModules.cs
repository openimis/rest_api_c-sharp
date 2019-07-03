using OpenImis.Modules.UserModule;
using OpenImis.Modules.InsureeManagementModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.Modules.MasterDataManagementModule;
using OpenImis.Modules.InsureeModule;
using OpenImis.Modules.LoginModule;
using OpenImis.Modules.CoverageModule;

namespace OpenImis.Modules
{
    /// <summary>
    /// This interface serves to define a Service for the IMIS modules 
    /// </summary>
    public interface IImisModules
    {
        IInsureeModule GetInsureeModule();
        ILoginModule GetLoginModule();
        ICoverageModule GetCoverageModule();

        /// <summary>
        /// Creates and returns the user management module.
        /// </summary>
        /// <returns>
        /// The User module.
        /// </returns>
        IUserModule GetUserModule();

        IInsureeManagementModule GetInsureeManagementModule();

		IMasterDataManagementModule GetMasterDataManagementModule();

    }
}
