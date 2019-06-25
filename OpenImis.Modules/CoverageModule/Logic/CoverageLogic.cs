using Microsoft.Extensions.Configuration;
using OpenImis.Modules.CoverageModule.Models;
using OpenImis.Modules.CoverageModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Logic
{
    public class CoverageLogic : ICoverageLogic
    {
        private IConfiguration Configuration;
        protected ICoverageRepository coverageRepository;

        public CoverageLogic(IConfiguration configuration)
        {
            Configuration = configuration;
            coverageRepository = new CoverageRepository(Configuration);
        }

        public CoverageModel Get(string insureeNumber)
        {
            CoverageModel response;

            response = coverageRepository.Get(insureeNumber);

            return response;
        }
    }
}
