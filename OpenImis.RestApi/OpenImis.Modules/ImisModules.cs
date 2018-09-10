//using OpenImis.RestApi.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.Modules;
using OpenImis.Modules.UserModule;
using OpenImis.Modules.WSModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenImis.Modules
{
    /// <summary>
    /// This Service instanciates the modules that will be used in the application entry point 
    /// </summary>
    public class ImisModules: IImisModules
    {
        private IUserModule _userModule;
        private IWSModule _wsModule;

		private readonly IConfiguration _configuration;
		private readonly ILogger logger;

		public ImisModules(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
			_configuration = configuration;
			logger = loggerFactory.CreateLogger("LoggerCategory");
		}

		private Type CreateTypeFromConfiguration(string moduleName, string sectionName, string defaultValue)
		{
			Type type;

			string part = "";

			if (_configuration.GetSection("ImisModules:" + moduleName).Exists())
			{
				var configSection = _configuration.GetSection("ImisModules:" + moduleName);

				if (configSection[sectionName] != null)
				{
					part = configSection[sectionName];
				}
			}

			type = Assembly.GetEntryAssembly().GetType(part);

			if (type == null)
			{
				logger.LogError(moduleName + " " + sectionName + " error: the type " + part + " was not found. Using default " + defaultValue + " configuration.");
				type = Assembly.GetEntryAssembly().GetType(defaultValue);
			}
			else
			{
				logger.LogInformation(moduleName + " load OK: " + part);
			}

			return type;
		}

        /// <summary>
        /// Return the user module
        /// </summary>
        /// <returns>
        /// The instance of the user repository
        /// </returns>
        public IUserModule getUserModule()
        {
			if (_userModule == null)
			{
				Type moduleType = CreateTypeFromConfiguration("UserModule", "Controllers", "OpenImis.Modules.UserModule.UserModule");
				Type repositoriesType = CreateTypeFromConfiguration("UserModule", "Repositories", "OpenImis.Modules.UserModule.UserModuleRepositories");
				
				_userModule = (IUserModule)Activator.CreateInstance(moduleType, (IUserModuleRepositories)Activator.CreateInstance(repositoriesType));
			}
			return _userModule;
        }

		public IWSModule getWSModule()
		{
			if (_wsModule == null)
			{
				Type moduleType = CreateTypeFromConfiguration("WSModule", "Controllers", "OpenImis.Modules.WSModule.WSModule");
				Type repositoriesType = CreateTypeFromConfiguration("WSModule", "Repositories", "OpenImis.Modules.WSModule.WSModuleRepositories");
				Type validatorsType = CreateTypeFromConfiguration("WSModule", "Validators", "OpenImis.Modules.WSModule.WSValidators");

				_wsModule = (IWSModule)Activator.CreateInstance(moduleType, (IWSModuleRepositories)Activator.CreateInstance(repositoriesType), (IWSValidators)Activator.CreateInstance(validatorsType));

			}
			return _wsModule;
		}
	}
}
