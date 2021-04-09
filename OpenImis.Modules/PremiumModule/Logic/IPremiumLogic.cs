using OpenImis.Modules.PremiumModule.Models;
using System;

namespace OpenImis.Modules.PremiumModule.Logic
{
    public interface IPremiumLogic
    {
        bool Get(ReceiptRequestModel receipt);
    }
}
