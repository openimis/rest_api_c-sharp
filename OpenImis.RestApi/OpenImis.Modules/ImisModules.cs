using Microsoft.Extensions.Logging;
using OpenImis.Modules.UserModule;
using OpenImis.Modules.UserModule.Controllers;
using OpenImis.Modules.WSModule;
using OpenImis.Modules.WSModule.Controllers;
using OpenImis.RestApi.OpenImis.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenImis.Modules
{
	/// <summary>
	/// This Service creates the entry point to the modules that will be used in the application entry point. 
	/// It reads the openImisModules.json configuration file and instanciates the desired controllers for each module. 
	/// Example of the configuration file:
	/// {
	///   "ImisModules": {
	///     "UserModule": {
	///       "UserController": "OpenImis.Modules.UserModule.Controllers.UserController"
	///     },
	///     "WSModule": {
	///       "FamilyController": "OpenImis.Modules.WSModule.Controllers.FamilyController",
	///       "InsureeController": "OpenImis.Modules.WSModule.Controllers.InsureeController"
	///     }
	///   }
	/// }
	/// </summary>
	public class ImisModules: IImisModules
    {
        private IUserModule _userModule;
        private IWSModule _wsModule;

		private readonly ILogger logger;

		public ImisModules(ILoggerFactory loggerFactory)
        {
			logger = loggerFactory.CreateLogger("LoggerCategory");
		}

		/// <summary>
        /// Return the user module
        /// </summary>
        /// <returns>
        /// The instance of the user repository
        /// </returns>
        public IUserModule GetUserModule()
        {
			if (_userModule == null)
			{
				_userModule = new UserModule.UserModule();
				Type userControllerType = Generators.CreateTypeFromConfiguration("UserModule", "UserController", "OpenImis.Modules.UserModule.Controllers.UserController", logger);
				
				_userModule.SetUserController((IUserController)Activator.CreateInstance(userControllerType));
			}
			return _userModule;
        }

		public IWSModule GetWSModule()
		{
			if (_wsModule == null)
			{
				_wsModule = new WSModule.WSModule();

				Type insureeControllerType = Generators.CreateTypeFromConfiguration("WSModule", "InsureeController", "OpenImis.Modules.WSModule.Controllers.InsureeController", logger);
				_wsModule.SetInsureeController((IInsureeController)Activator.CreateInstance(insureeControllerType));

				Type familyControllerType = Generators.CreateTypeFromConfiguration("WSModule", "FamilyController", "OpenImis.Modules.WSModule.Controllers.FamilyController", logger);
				_wsModule.SetFamilyController((IFamilyController)Activator.CreateInstance(familyControllerType));

			}
			return _wsModule;
		}
	}
}
