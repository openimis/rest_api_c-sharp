using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV1.InsureeModule.Models;
using OpenImis.ModulesV1.InsureeModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Logic
{
    public class ContributionLogic : IContributionLogic
    {
        private IConfiguration Configuration;
        protected IContributionRepository contributionRepository;

        public ContributionLogic(IConfiguration configuration)
        {
            Configuration = configuration;
            contributionRepository = new ContributionRepository(Configuration);
        }

        public void SetUserId(int userId)
        {
            contributionRepository.UserId = userId;
        }

        public DataMessage Enter(Contribution model)
        {
            DataMessage message;

            message = contributionRepository.Enter(model);

            return message;
        }
    }
}
