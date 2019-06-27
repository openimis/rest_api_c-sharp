using Microsoft.Extensions.Configuration;
using OpenImis.Modules.CoverageModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule
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
    }
}
