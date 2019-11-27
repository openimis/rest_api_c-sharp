using OpenImis.ModulesV1.PaymentModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule
{
    public interface IPaymentModule
    {
        IPaymentLogic GetPaymentLogic();
        IPaymentModule SetPaymentLogic(IPaymentLogic paymentLogic);
    }
}
