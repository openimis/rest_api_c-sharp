using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.CoverageModule.Logic;

namespace OpenImis.ModulesV3.CoverageModule
{
    public class CoverageModule : ICoverageModule
    {
        private IConfiguration _configuration;
        private ICoverageLogic _coverageLogic;

        public CoverageModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ICoverageLogic GetCoverageLogic()
        {
            if (_coverageLogic == null)
            {
                _coverageLogic = new CoverageLogic(_configuration);
            }
            return _coverageLogic;
        }

        public ICoverageModule SetCoverageLogic(ICoverageLogic coverageLogic)
        {
            _coverageLogic = coverageLogic;
            return this;
        }
    }
}
