using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Repositories;

namespace OpenImis.ModulesV3.InsureeModule.Logic
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

        public GetEnquireModel GetEnquire(string chfid)
        {
            GetEnquireModel response;

            response = insureeRepository.GetEnquire(chfid);

            return response;
        }

        public GetInsureeModel Get(string chfid)
        {
            GetInsureeModel response;

            response = insureeRepository.Get(chfid);

            return response;
        }
    }
}
