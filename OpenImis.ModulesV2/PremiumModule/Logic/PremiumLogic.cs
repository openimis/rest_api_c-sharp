using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.PremiumModule.Models;
using OpenImis.ModulesV2.PremiumModule.Repositories;

namespace OpenImis.ModulesV2.PremiumModule.Logic
{
    public class PremiumLogic : IPremiumLogic
    {
        private IConfiguration _configuration;

        protected IPremiumRepository premiumRepository;

        public PremiumLogic(IConfiguration configuration)
        {
            _configuration = configuration;

            premiumRepository = new PremiumRepository(_configuration);
        }

        public bool Get(ReceiptRequestModel receipt)
        {
            bool response;

            response = premiumRepository.Get(receipt);

            return response;
        }
    }
}
