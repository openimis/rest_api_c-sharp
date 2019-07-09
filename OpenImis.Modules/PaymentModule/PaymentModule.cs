using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules.PaymentModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule
{
    public class PaymentModule : IPaymentModule
    {
        private IConfiguration Configuration;
        private IPaymentLogic _paymentLogic;

        public PaymentModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IPaymentLogic GetPaymentLogic()
        {
            if (_paymentLogic == null)
            {
                _paymentLogic = new PaymentLogic(Configuration);
            }
            return _paymentLogic;
        }
    }
}
