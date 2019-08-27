using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.InsureeModule.Models;
using OpenImis.ModulesV2.InsureeModule.Repositories;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public class InsureeLogic : IInsureeLogic
    {
        private IConfiguration _configuration;
        protected IInsureeRepository insureeRepository;

        public InsureeLogic(IConfiguration configuration)
        {
            _configuration = configuration;

            insureeRepository = new InsureeRepository(_configuration);
        }

        public GetInsureeModel Get(string chfid)
        {
            GetInsureeModel response;

            response = insureeRepository.Get(chfid);

            return response;
        }
    }
}
