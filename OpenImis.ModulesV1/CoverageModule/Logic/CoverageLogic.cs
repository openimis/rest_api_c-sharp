using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV1.CoverageModule.Models;
using OpenImis.ModulesV1.CoverageModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule.Logic
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
