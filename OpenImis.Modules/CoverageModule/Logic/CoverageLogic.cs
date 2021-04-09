using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Logic
{
    public class CoverageLogic : ICoverageLogic
    {
        private IConfiguration _configuration;

        public CoverageLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
