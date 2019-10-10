using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.InsureeModule.Logic;

namespace OpenImis.ModulesV2.InsureeModule
{
    public class InsureeModule : IInsureeModule
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        private IFamilyLogic _familyLogic;
        private IContributionLogic _contributionLogic;

        public InsureeModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IFamilyLogic GetFamilyLogic()
        {
            if (_familyLogic == null)
            {
                _familyLogic = new FamilyLogic(_configuration, _hostingEnvironment);
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

        public IInsureeModule SetFamilyLogic(IFamilyLogic familyLogic)
        {
            _familyLogic = familyLogic;
            return this;
        }

        public IInsureeModule SetContributionLogic(IContributionLogic contributionLogic)
        {
            _contributionLogic = contributionLogic;
            return this;
        }
    }
}
