﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.InsureeModule.Models;
using OpenImis.ModulesV2.InsureeModule.Repositories;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public class PolicyLogic : IPolicyLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        protected IPolicyRepository policyRepository;

        public PolicyLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            policyRepository = new PolicyRepository(_configuration, _hostingEnvironment);
        }

        public List<GetPolicyModel> Get(string officerCode)
        {
            List<GetPolicyModel> response;

            response = policyRepository.Get(officerCode);

            return response;
        }

        public int Post(PolicyRenewalModel policy)
        {
            int response;

            response = policyRepository.Post(policy);

            return response;
        }

        public int Delete(Guid uuid)
        {
            int response;

            response = policyRepository.Delete(uuid);

            return response;
        }

        public string GetLoginNameByUserUUID(Guid userUUID)
        {
            string response;

            response = policyRepository.GetLoginNameByUserUUID(userUUID);

            return response;
        }
    }
}
