using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3.PolicyModule.Models;
using OpenImis.ModulesV3.PolicyModule.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.PolicyModule.Logic
{
    public class PolicyRenewalLogic : IPolicyRenewalLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ILoggerFactory _loggerFactory { get; }

        protected IPolicyRenewalRepository policyRenewalRepository;

        public PolicyRenewalLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;

            policyRenewalRepository = new PolicyRenewalRepository(_configuration, _hostingEnvironment, _loggerFactory);
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

        public DataMessage GetCommissions(GetCommissionInputs model)
        {
            DataMessage response;

            response = policyRenewalRepository.GetCommissions(model);

            return response;
        }

        public async Task<DataMessage> SelfRenewal(SelfRenewal renewal)
        {
            DataMessage response = await policyRenewalRepository.SelfRenewal(renewal);
            return response;
        }
    }
}
