using Microsoft.Extensions.Configuration;
using OpenImis.Modules.InsureeModule.Models;
using OpenImis.Modules.InsureeModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Logic
{
    public class PolicyLogic : IPolicyLogic
    {
        private IConfiguration Configuration;
        protected IPolicyRepository policyRepository;

        public PolicyLogic(IConfiguration configuration)
        {
            Configuration = configuration;
            policyRepository = new PolicyRepository(Configuration);
        }

        public void SetUserId(int userId)
        {
            policyRepository.UserId = userId;
        }

        public DataMessage Enter(Policy model)
        {
            DataMessage message;

            message = policyRepository.Enter(model);

            return message;
        }

        public DataMessage Renew(Policy model)
        {
            DataMessage message;

            message = policyRepository.Renew(model);

            return message;
        }

        public DataMessage GetCommissions(GetCommissionInputs model)
        {
            DataMessage message;

            message = policyRepository.GetCommissions(model);

            return message;
        }
    }
}
