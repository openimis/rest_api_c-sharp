using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV1.CoverageModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule
{
    public class CoverageModule : ICoverageModule
    {
        private IConfiguration Configuration;
        private ICoverageLogic _coverageLogic;

        public CoverageModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ICoverageLogic GetCoverageLogic()
        {
            if (_coverageLogic == null)
            {
                _coverageLogic = new CoverageLogic(Configuration);
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
