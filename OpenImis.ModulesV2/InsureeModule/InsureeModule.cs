using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.InsureeModule.Logic;

namespace OpenImis.ModulesV2.InsureeModule
{
    public class InsureeModule : IInsureeModule
    {
        private IConfiguration _configuration;

        private IFamilyLogic _familyLogic;
        private IContributionLogic _contributionLogic;
        private IPolicyLogic _policyLogic;

        public InsureeModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IFamilyLogic GetFamilyLogic()
        {
            if (_familyLogic == null)
            {
                _familyLogic = new FamilyLogic(_configuration);
            }
            return _familyLogic;
        }

        public IContributionLogic GetContributionLogic()
        {
            if (_contributionLogic == null)
            {
                _contributionLogic = new ContributionLogic(_configuration);
            }
            return _contributionLogic;
        }

        public IPolicyLogic GetPolicyLogic()
        {
            if (_policyLogic == null)
            {
                _policyLogic = new PolicyLogic(_configuration);
            }
            return _policyLogic;
        }

        public IInsureeModule SetFamilyLogic(IFamilyLogic familyLogic)
        {
            _familyLogic = familyLogic;
            return this;
        }

        public IInsureeModule SetPolicyLogic(IPolicyLogic policyLogic)
        {
            _policyLogic = policyLogic;
            return this;
        }

        public IInsureeModule SetContributionLogic(IContributionLogic contributionLogic)
        {
            _contributionLogic = contributionLogic;
            return this;
        }
    }
}
