using System;
using System.Linq;
using System.Reflection;
using OpenImis.Modules.Helpers;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenImis.Modules.LoginModule;
using OpenImis.Modules.ClaimModule;
using OpenImis.Modules.InsureeModule;
using OpenImis.Modules.CoverageModule;
using OpenImis.Modules.PaymentModule;
using OpenImis.Modules.MasterDataModule;
using OpenImis.Modules.MasterDataModule.Logic;
using OpenImis.Modules.FeedbackModule;
using Microsoft.AspNetCore.Hosting;
using OpenImis.Modules.PremiumModule;
using OpenImis.Modules.SystemModule;
using OpenImis.Modules.PolicyModule;
using OpenImis.Modules.PolicyModule.Logic;
using OpenImis.Modules.ReportModule;
using OpenImis.Modules.ReportModule.Logic;

namespace OpenImis.Modules
{
    public class ImisModules : IImisModules
    {
        private ILoginModule loginModule;

        private IInsureeModule insureeModule;
        private IClaimModule claimModule;
        private ICoverageModule coverageModule;
        private IPaymentModule paymentModule;
        private IMasterDataModule masterDataModule;
        private IFeedbackModule feedbackModule;
        private IPremiumModule premiumModule;
        private ISystemModule systemModule;
        private IPolicyModule policyModule;
        private IReportModule reportModule;

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public ImisModules(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _logger = loggerFactory.CreateLogger("LoggerCategory");
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates and returns the login module version 2.
        /// </summary>
        /// <returns>
        /// The Login module V2.
        /// </returns>
        public ILoginModule GetLoginModule()
        {
            if (loginModule == null)
            {
                loginModule = new LoginModule.LoginModule(_configuration);

                Type loginLogicType = CreateTypeFromConfiguration("LoginModule", "LoginLogic", "OpenImis.Modules.LoginModule.Logic.LoginLogic");
                loginModule.SetLoginLogic((LoginModule.Logic.ILoginLogic)ActivatorUtilities.CreateInstance(_serviceProvider, loginLogicType));
            }
            return loginModule;
        }

        /// <summary>
        /// Creates and returns the claim module version 2.
        /// </summary>
        /// <returns>
        /// The Claim module V2.
        /// </returns>
        public IClaimModule GetClaimModule()
        {
            if (claimModule == null)
            {
                claimModule = new ClaimModule.ClaimModule(_configuration, _hostingEnvironment);

                Type claimLogicType = CreateTypeFromConfiguration("ClaimModule", "ClaimLogic", "OpenImis.Modules.ClaimModule.Logic.ClaimLogic");
                claimModule.SetClaimLogic((ClaimModule.Logic.IClaimLogic)ActivatorUtilities.CreateInstance(_serviceProvider, claimLogicType));
            }
            return claimModule;
        }

        /// <summary>
        /// Creates and returns the insuree module version 2.
        /// </summary>
        /// <returns>
        /// The Insuree module V2.
        /// </returns>
        public IInsureeModule GetInsureeModule()
        {
            if (insureeModule == null)
            {
                insureeModule = new InsureeModule.InsureeModule(_configuration, _hostingEnvironment);

                Type familyLogicType = CreateTypeFromConfiguration("InsureeModule", "FamilyLogic", "OpenImis.Modules.InsureeModule.Logic.FamilyLogic");
                insureeModule.SetFamilyLogic((InsureeModule.Logic.IFamilyLogic)ActivatorUtilities.CreateInstance(_serviceProvider, familyLogicType));

                Type contributionLogicType = CreateTypeFromConfiguration("InsureeModule", "ContributionLogic", "OpenImis.Modules.InsureeModule.Logic.ContributionLogic");
                insureeModule.SetContributionLogic((InsureeModule.Logic.IContributionLogic)ActivatorUtilities.CreateInstance(_serviceProvider, contributionLogicType));

                Type insureeLogicType = CreateTypeFromConfiguration("InsureeModule", "InsureeLogic", "OpenImis.Modules.InsureeModule.Logic.InsureeLogic");
                insureeModule.SetInsureeLogic((InsureeModule.Logic.IInsureeLogic)ActivatorUtilities.CreateInstance(_serviceProvider, insureeLogicType));
            }
            return insureeModule;
        }

        /// <summary>
        /// Creates and returns the payment module version 2.
        /// </summary>
        /// <returns>
        /// The Payment module V2.
        /// </returns>
        public ICoverageModule GetCoverageModule()
        {
            if (coverageModule == null)
            {
                coverageModule = new CoverageModule.CoverageModule(_configuration);

                Type coverageLogicType = CreateTypeFromConfiguration("CoverageModule", "CoverageLogic", "OpenImis.Modules.CoverageModule.Logic.CoverageLogic");
                coverageModule.SetCoverageLogic((CoverageModule.Logic.ICoverageLogic)ActivatorUtilities.CreateInstance(_serviceProvider, coverageLogicType));
            }
            return coverageModule;
        }

        /// <summary>
        /// Creates and returns the payment module version 2.
        /// </summary>
        /// <returns>
        /// The Payment module V2.
        /// </returns>
        public IPaymentModule GetPaymentModule()
        {
            if (paymentModule == null)
            {
                paymentModule = new PaymentModule.PaymentModule(_configuration);

                Type paymentLogicType = CreateTypeFromConfiguration("PaymentModule", "PaymentLogic", "OpenImis.Modules.PaymentModule.Logic.PaymentLogic");
                paymentModule.SetPaymentLogic((PaymentModule.Logic.IPaymentLogic)ActivatorUtilities.CreateInstance(_serviceProvider, paymentLogicType));
            }
            return paymentModule;
        }

        /// <summary>
        /// Creates and returns the feedback module version 2.
        /// </summary>
        /// <returns>
        /// The Feedback module V2.
        /// </returns>
        public IFeedbackModule GetFeedbackModule()
        {
            if (feedbackModule == null)
            {
                feedbackModule = new FeedbackModule.FeedbackModule(_configuration, _hostingEnvironment);

                Type feedbackLogicType = CreateTypeFromConfiguration("FeedbackModule", "FeedbackLogic", "OpenImis.Modules.FeedbackModule.Logic.FeedbackLogic");
                feedbackModule.SetFeedbackLogic((FeedbackModule.Logic.IFeedbackLogic)ActivatorUtilities.CreateInstance(_serviceProvider, feedbackLogicType));
            }
            return feedbackModule;
        }

        /// <summary>
        /// Creates and returns the premium module version 2.
        /// </summary>
        /// <returns>
        /// The Premium module V2.
        /// </returns>
        public IPremiumModule GetPremiumModule()
        {
            if (premiumModule == null)
            {
                premiumModule = new PremiumModule.PremiumModule(_configuration);

                Type premiumLogicType = CreateTypeFromConfiguration("PremiumModule", "PremiumLogic", "OpenImis.Modules.PremiumModule.Logic.PremiumLogic");
                premiumModule.SetPremiumLogic((PremiumModule.Logic.IPremiumLogic)ActivatorUtilities.CreateInstance(_serviceProvider, premiumLogicType));
            }
            return premiumModule;
        }

        /// <summary>
        /// Creates and returns the system module version 2.
        /// </summary>
        /// <returns>
        /// The System module V2.
        /// </returns>
        public ISystemModule GetSystemModule()
        {
            if (systemModule == null)
            {
                systemModule = new SystemModule.SystemModule(_configuration);

                Type systemLogicType = CreateTypeFromConfiguration("SystemModule", "SystemLogic", "OpenImis.Modules.SystemModule.Logic.SystemLogic");
                systemModule.SetSystemLogic((SystemModule.Logic.ISystemLogic)ActivatorUtilities.CreateInstance(_serviceProvider, systemLogicType));
            }
            return systemModule;
        }

        /// <summary>
        /// Creates and returns the master data module version 2.
        /// </summary>
        /// <returns>
        /// The MasterData module V2.
        /// </returns>
        public IMasterDataModule GetMasterDataModule()
        {
            if (masterDataModule == null)
            {
                masterDataModule = new MasterDataModule.MasterDataModule(_configuration);

                Type masterDataLogicType = CreateTypeFromConfiguration("MasterDataModule", "MasterDataLogic", "OpenImis.Modules.MasterDataModule.Logic.MasterDataLogic");
                masterDataModule.SetMasterDataLogic((IMasterDataLogic)ActivatorUtilities.CreateInstance(_serviceProvider, masterDataLogicType));
            }
            return masterDataModule;
        }

        /// <summary>
        /// Creates and returns the policy module version 2.
        /// </summary>
        /// <returns>
        /// The Policy module V2.
        /// </returns>
        public IPolicyModule GetPolicyModule()
        {
            if (policyModule == null)
            {
                policyModule = new PolicyModule.PolicyModule(_configuration, _hostingEnvironment);

                Type policyLogicType = CreateTypeFromConfiguration("PolicyModule", "PolicyRenewalLogic", "OpenImis.Modules.PolicyModule.Logic.PolicyRenewalLogic");
                policyModule.SetPolicyLogic((IPolicyRenewalLogic)ActivatorUtilities.CreateInstance(_serviceProvider, policyLogicType));
            }
            return policyModule;
        }

        /// Creates and returns the report module version 2.
        /// </summary>
        /// <returns>
        /// The Report module V2.
        /// </returns>
        public IReportModule GetReportModule()
        {
            if (reportModule == null)
            {
                reportModule = new ReportModule.ReportModule(_configuration);

                Type reportLogicType = CreateTypeFromConfiguration("ReportModule", "ReportLogic", "OpenImis.Modules.ReportModule.Logic.ReportLogic");
                reportModule.SetReportLogic((IReportLogic)ActivatorUtilities.CreateInstance(_serviceProvider, reportLogicType));
            }
            return reportModule;
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

            Assembly assembly = Assembly.GetCallingAssembly();
            string part = GetSectionName(moduleName, sectionName, "2");
            type = assembly.GetType(part);
            if (type == null)
            {
                _logger.LogError(moduleName + " " + sectionName + " error: the type " + part + " was not found. Using default " + defaultValue + " configuration.");
                type = assembly.GetType(defaultValue);
            }
            else
            {
                _logger.LogInformation(moduleName + " load OK: " + part);
            }

            return type;
        }

        public string GetSectionName(string moduleName, string sectionName, string apiVersion)
        {
            string part = "";

            var listImisModules = _configuration.GetSection("ImisModules").Get<List<ConfigImisModules>>();

            var module = listImisModules.Where(m => m.Version == apiVersion).Select(x => GetPropValue(x, moduleName)).FirstOrDefault();

            if (GetPropValue(module, sectionName) != null)
            {
                part = GetPropValue(module, sectionName).ToString();
            }
            return part;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
