using Microsoft.Extensions.Logging;
using OpenImis.Modules.UserModule;
using OpenImis.Modules.UserModule.Controllers;
using OpenImis.Modules.InsureeManagementModule;
using OpenImis.Modules.InsureeManagementModule.Logic;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules.MasterDataManagementModule;
using OpenImis.Modules.MasterDataManagementModule.Logic;
using OpenImis.Modules.ClaimModule;
using OpenImis.Modules.InsureeModule;
using OpenImis.Modules.LoginModule;
using OpenImis.Modules.CoverageModule;
using OpenImis.Modules.PaymentModule;
using Microsoft.AspNetCore.Hosting;

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

		private IUserModule userModule;
        private IInsureeManagementModule insureeManagementModule;
        private IMasterDataManagementModule masterDataManagementModule;

        private IClaimModule claimModule;
        private IInsureeModule insureeModule;
        private ILoginModule loginModule;
        private ICoverageModule coverageModule;
        private IPaymentModule paymentModule;

        private readonly IConfiguration _configuration;
        private readonly ILogger logger;

		public ImisModules(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
			_configuration = configuration;
            logger = loggerFactory.CreateLogger("LoggerCategory"); 
		}

        public IClaimModule GetClaimModule()
        {
            if (claimModule == null)
            {
                claimModule = new ClaimModule.ClaimModule();
            }
            return claimModule;
        }

        public IInsureeModule GetInsureeModule()
        {
            if (insureeModule == null)
            {
                insureeModule = new InsureeModule.InsureeModule(_configuration);
            }
            return insureeModule;
        }

        public ILoginModule GetLoginModule()
        {
            if (loginModule == null)
            {
                loginModule = new LoginModule.LoginModule(_configuration);
            }
            return loginModule;
        }

        public ICoverageModule GetCoverageModule()
        {
            if (coverageModule == null)
            {
                coverageModule = new CoverageModule.CoverageModule(_configuration);
            }
            return coverageModule;
        }

        public IPaymentModule GetPaymentModule()
        {
            if (paymentModule == null)
            {
                paymentModule = new PaymentModule.PaymentModule(_configuration);
            }
            return paymentModule;
        }

        /// <summary>
        /// Creates and returns the user management module.
        /// </summary>
        /// <returns>
        /// The User module.
        /// </returns>
        public IUserModule GetUserModule()
        {
			if (userModule == null)
			{
				userModule = new UserModule.UserModule();
				Type userControllerType = CreateTypeFromConfiguration("UserModule", "UserController", "OpenImis.Modules.UserModule.Controllers.UserController");
				
				userModule.SetUserController((IUserController)Activator.CreateInstance(userControllerType));
			} 
			return userModule;
        }

		/// <summary>
		/// Creates and returns the Web Services integration module.
		/// </summary>
		/// <returns>
		/// The Web Services integration module.
		/// </returns>
		public IInsureeManagementModule GetInsureeManagementModule()
		{
			if (insureeManagementModule == null)
			{
				insureeManagementModule = new InsureeManagementModule.InsureeManagementModule(this);

				Type insureeLogicType = CreateTypeFromConfiguration("InsureeManagementModule", "InsureeLogic", "OpenImis.Modules.InsureeManagementModule.Logic.InsureeLogic");
				insureeManagementModule.SetInsureeLogic((IInsureeLogic)Activator.CreateInstance(insureeLogicType, this));

				Type familyLogicType = CreateTypeFromConfiguration("InsureeManagementModule", "FamilyLogic", "OpenImis.Modules.InsureeManagementModule.Logic.FamilyLogic");
				insureeManagementModule.SetFamilyLogic((IFamilyLogic)Activator.CreateInstance(familyLogicType, this));

			}
			return insureeManagementModule;
		}

		/// <summary>
		/// Creates and returns the Web Services integration module.
		/// </summary>
		/// <returns>
		/// The Web Services integration module.
		/// </returns>
		public IMasterDataManagementModule GetMasterDataManagementModule()
		{
			if (masterDataManagementModule == null)
			{
				masterDataManagementModule = new MasterDataManagementModule.MasterDataManagementModule(this);

				Type locationLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "LocationLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.LocationLogic");
				masterDataManagementModule.SetLocationLogic((ILocationLogic)Activator.CreateInstance(locationLogicType, this));

				Type familyTypeLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "FamilyTypeLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.FamilyTypeLogic");
				masterDataManagementModule.SetFamilyTypeLogic((IFamilyTypeLogic)Activator.CreateInstance(familyTypeLogicType, this));

				Type confirmationTypeLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "ConfirmationTypeLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.ConfirmationTypeLogic");
				masterDataManagementModule.SetConfirmationTypeLogic((IConfirmationTypeLogic)Activator.CreateInstance(confirmationTypeLogicType, this));

				Type educationLevelLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "EducationLevelLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.EducationLevelLogic");
				masterDataManagementModule.SetEducationLevelLogic((IEducationLevelLogic)Activator.CreateInstance(educationLevelLogicType, this));

				Type genderTypeLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "GenderTypeLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.GenderTypeLogic");
				masterDataManagementModule.SetGenderTypeLogic((IGenderTypeLogic)Activator.CreateInstance(genderTypeLogicType, this));

				Type professionTypeLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "ProfessionTypeLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.ProfessionTypeLogic");
				masterDataManagementModule.SetProfessionTypeLogic((IProfessionTypeLogic)Activator.CreateInstance(professionTypeLogicType, this));

				Type identificationTypeLogicType = CreateTypeFromConfiguration("MasterDataManagementModule", "IdentificationTypeLogic", "OpenImis.Modules.MasterDataManagementModule.Logic.IdentificationTypeLogic");
				masterDataManagementModule.SetIdentificationTypeLogic((IIdentificationTypeLogic)Activator.CreateInstance(identificationTypeLogicType, this));

			}
			return masterDataManagementModule;
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
