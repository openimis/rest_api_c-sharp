using OpenImis.ModulesV2.PremiumModule.Models;
using System;

namespace OpenImis.ModulesV2.PremiumModule.Logic
{
    public interface IPremiumLogic
    {
        bool Get(ReceiptRequestModel receipt);
    }
}
