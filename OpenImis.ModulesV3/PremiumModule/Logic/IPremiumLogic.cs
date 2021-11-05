using OpenImis.ModulesV3.PremiumModule.Models;
using System;

namespace OpenImis.ModulesV3.PremiumModule.Logic
{
    public interface IPremiumLogic
    {
        bool Get(ReceiptRequestModel receipt);
    }
}
