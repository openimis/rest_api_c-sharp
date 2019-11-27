using OpenImis.ModulesV2.PaymentModule.Logic;
using System;

namespace OpenImis.ModulesV2.PaymentModule
{
    public interface IPaymentModule
    {
        IPaymentLogic GetPaymentLogic();
        IPaymentModule SetPaymentLogic(IPaymentLogic paymentLogic);
    }
}
