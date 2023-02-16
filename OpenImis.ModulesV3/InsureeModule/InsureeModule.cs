using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3.InsureeModule.Logic;

namespace OpenImis.ModulesV3.InsureeModule
{
    public class InsureeModule : IInsureeModule
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        private IFamilyLogic _familyLogic;
        private IContributionLogic _contributionLogic;
        private IInsureeLogic _insureeLogic;
        private ILoggerFactory _loggerFactory;

        public InsureeModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public IFamilyLogic GetFamilyLogic()
        {
            if (_familyLogic == null)
            {
                _familyLogic = new FamilyLogic(_configuration, _hostingEnvironment, _loggerFactory);
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

        public IInsureeLogic GetInsureeLogic()
        {
            if (_insureeLogic == null)
            {
                _insureeLogic = new InsureeLogic(_configuration, _loggerFactory);
            }
            return _insureeLogic;
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

        public IInsureeModule SetInsureeLogic(IInsureeLogic insureeLogic)
        {
            _insureeLogic = insureeLogic;
            return this;
        }
    }
}
