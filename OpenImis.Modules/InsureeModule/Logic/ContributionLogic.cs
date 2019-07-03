using Microsoft.Extensions.Configuration;
using OpenImis.Modules.InsureeModule.Models;
using OpenImis.Modules.InsureeModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Logic
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
