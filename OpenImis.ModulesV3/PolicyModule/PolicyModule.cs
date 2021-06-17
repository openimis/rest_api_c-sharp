using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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

        private IPolicyRenewalLogic _policyRenewalLogic;

        public PolicyModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IPolicyRenewalLogic GetPolicyRenewalLogic()
        {
            if (_policyRenewalLogic == null)
            {
                _policyRenewalLogic = new PolicyRenewalLogic(_configuration, _hostingEnvironment);
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
