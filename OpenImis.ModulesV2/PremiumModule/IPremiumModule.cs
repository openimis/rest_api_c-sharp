using OpenImis.ModulesV2.PremiumModule.Logic;

namespace OpenImis.ModulesV2.PremiumModule
{
    public interface IPremiumModule
    {
        IPremiumLogic GetPremiumLogic();
        IPremiumModule SetPremiumLogic(IPremiumLogic premiumLogic);
    }
}
