using Microsoft.Extensions.Configuration;
using OpenImis.Modules.PremiumModule.Models;
using OpenImis.Modules.PremiumModule.Repositories;

namespace OpenImis.Modules.PremiumModule.Logic
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
