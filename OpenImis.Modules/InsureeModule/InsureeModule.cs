using Microsoft.Extensions.Configuration;
using OpenImis.Modules.InsureeModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule
{
    public class InsureeModule : IInsureeModule
    {
        private IConfiguration Configuration;
        private IFamilyLogic _familyLogic;
        private IContributionLogic _contributionLogic;
        private IPolicyLogic _policyLogic;

        public InsureeModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IFamilyLogic GetFamilyLogic()
        {
            if (_familyLogic == null)
            {
                _familyLogic = new FamilyLogic(Configuration);
            }
            return _familyLogic;
        }

        public IContributionLogic GetContributionLogic()
        {
            if (_contributionLogic == null)
            {
                _contributionLogic = new ContributionLogic(Configuration);
            }
            return _contributionLogic;
        }

        public IPolicyLogic GetPolicyLogic()
        {
            if (_policyLogic == null)
            {
                _policyLogic = new PolicyLogic(Configuration);
            }
            return _policyLogic;
        }
    }
}
