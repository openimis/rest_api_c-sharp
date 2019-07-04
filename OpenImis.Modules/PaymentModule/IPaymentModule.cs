using OpenImis.Modules.PaymentModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule
{
    public interface IPaymentModule
    {
        IPaymentLogic GetPaymentLogic();
    }
}
