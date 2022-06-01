using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3.PolicyModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.PolicyModule
{
    public class PolicyModule : IPolicyModule
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;
        private IPolicyRenewalLogic _policyRenewalLogic;

        public PolicyModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public IPolicyRenewalLogic GetPolicyRenewalLogic()
        {
            if (_policyRenewalLogic == null)
            {
                _policyRenewalLogic = new PolicyRenewalLogic(_configuration, _hostingEnvironment, _loggerFactory);
            }
            return _policyRenewalLogic;
        }

        public IPolicyModule SetPolicyLogic(IPolicyRenewalLogic policyRenewalLogic)
        {
            _policyRenewalLogic = policyRenewalLogic;
            return this;
        }
    }
}
