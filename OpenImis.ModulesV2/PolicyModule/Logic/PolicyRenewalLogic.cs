using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.PolicyModule.Models;
using OpenImis.ModulesV2.PolicyModule.Repositories;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.PolicyModule.Logic
{
    public class PolicyRenewalLogic : IPolicyRenewalLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        protected IPolicyRenewalRepository policyRenewalRepository;

        public PolicyRenewalLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            policyRenewalRepository = new PolicyRenewalRepository(_configuration, _hostingEnvironment);
        }

        public List<GetPolicyRenewalModel> Get(string officerCode)
        {
            List<GetPolicyRenewalModel> response;

            response = policyRenewalRepository.Get(officerCode);

            return response;
        }

        public int Post(PolicyRenewalModel policy)
        {
            int response;

            response = policyRenewalRepository.Post(policy);

            return response;
        }

        public int Delete(Guid uuid)
        {
            int response;

            response = policyRenewalRepository.Delete(uuid);

            return response;
        }
    }
}
