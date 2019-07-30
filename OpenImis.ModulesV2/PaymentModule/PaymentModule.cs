using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.PaymentModule.Logic;

namespace OpenImis.ModulesV2.PaymentModule
{
    public class PaymentModule : IPaymentModule
    {
        private IConfiguration _configuration;
        private IPaymentLogic _paymentLogic;

        public PaymentModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IPaymentLogic GetPaymentLogic()
        {
            if (_paymentLogic == null)
            {
                _paymentLogic = new PaymentLogic(_configuration);
            }
            return _paymentLogic;
        }

        public IPaymentModule SetPaymentLogic(IPaymentLogic paymentLogic)
        {
            _paymentLogic = paymentLogic;
            return this;
        }
    }
}
