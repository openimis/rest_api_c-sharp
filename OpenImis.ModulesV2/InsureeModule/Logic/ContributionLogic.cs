using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public class ContributionLogic : IContributionLogic
    {
        private IConfiguration _configuration;

        public ContributionLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
