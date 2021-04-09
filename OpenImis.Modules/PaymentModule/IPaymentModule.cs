using OpenImis.Modules.PaymentModule.Logic;
using System;

namespace OpenImis.Modules.PaymentModule
{
    public interface IPaymentModule
    {
        IPaymentLogic GetPaymentLogic();
        IPaymentModule SetPaymentLogic(IPaymentLogic paymentLogic);
    }
}
