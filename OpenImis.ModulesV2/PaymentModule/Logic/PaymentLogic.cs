using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace OpenImis.ModulesV2.PaymentModule.Logic
{
    public class PaymentLogic : IPaymentLogic
    {
        private IConfiguration _configuration;

        public PaymentLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
