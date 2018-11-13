using Microsoft.Extensions.Logging;
using OpenImis.Modules.UserModule;
using OpenImis.Modules.UserModule.Controllers;
using OpenImis.Modules.InsureeManagementModule;
using OpenImis.Modules.InsureeManagementModule.Logic;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace OpenImis.Modules
{
	/// <summary>
	/// Provides the entry point to the services.
	/// </summary>
	/// <remarks>
	/// The modules and the associated controllers are constructed based on the configuratîon.
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
	/// </remarks>
	public class ImisModules: IImisModules
    {

		private IUserModule _userModule;
        private IInsureeManagementModule _wsModule;

		private readonly IConfiguration _configuration;
		private readonly ILogger logger;

		public ImisModules(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
			_configuration = configuration;
			logger = loggerFactory.CreateLogger("LoggerCategory"); 
		}

		/// <summary>
        /// Creates and returns the user management module.
        /// </summary>
        /// <returns>
        /// The User module.
        /// </returns>
        public IUserModule GetUserModule()
        {
			if (_userModule == null)
			{
				_userModule = new UserModule.UserModule();
				Type userControllerType = CreateTypeFromConfiguration("UserModule", "UserController", "OpenImis.Modules.UserModule.Controllers.UserController");
				
				_userModule.SetUserController((IUserController)Activator.CreateInstance(userControllerType));
			} 
			return _userModule;
        }

		/// <summary>
		/// Creates and returns the Web Services integration module.
		/// </summary>
		/// <returns>
		/// The Web Services integration module.
		/// </returns>
		public IInsureeManagementModule GetInsureeManagementModule()
		{
			if (_wsModule == null)
			{
				_wsModule = new InsureeManagementModule.WSModule();

				Type insureeControllerType = CreateTypeFromConfiguration("InsureeManagementModule", "InsureeLogic", "OpenImis.Modules.WSModule.Logic.InsureeLogic");
				_wsModule.SetInsureeLogic((IInsureeLogic)Activator.CreateInstance(insureeControllerType));

				Type familyControllerType = CreateTypeFromConfiguration("InsureeManagementModule", "FamilyLogic", "OpenImis.Modules.WSModule.Logic.FamilyLogic");
				_wsModule.SetFamilyLogic((IFamilyLogic)Activator.CreateInstance(familyControllerType));

			}
			return _wsModule;
		}

		/// <summary>
		/// Creates and returns the type based on the string from the configuration 
		/// </summary>
		/// <param name="moduleName">The module name</param>
		/// <param name="sectionName">The section name</param>
		/// <param name="defaultValue">The default section value</param>
		/// <returns>Type represented by the section</returns>
		private Type CreateTypeFromConfiguration(string moduleName, string sectionName, string defaultValue)
		{

			Type type;

			string part = "";

			Assembly assembly = Assembly.GetCallingAssembly();

			if (_configuration.GetSection("ImisModules:" + moduleName).Exists())
			{
				var configSection = _configuration.GetSection("ImisModules:" + moduleName);

				if (configSection[sectionName] != null)
				{
					part = configSection[sectionName];
				}
			}
			
			type = assembly.GetType(part);

			if (type == null)
			{
				logger.LogError(moduleName + " " + sectionName + " error: the type " + part + " was not found. Using default " + defaultValue + " configuration.");

				type = assembly.GetType(defaultValue);
			}
			else
			{
				logger.LogInformation(moduleName + " load OK: " + part);
			}

			return type;
		}
	}
}
