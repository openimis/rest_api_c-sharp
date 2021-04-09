using Microsoft.Extensions.Configuration;
using OpenImis.Modules.PaymentModule.Logic;

namespace OpenImis.Modules.PaymentModule
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
