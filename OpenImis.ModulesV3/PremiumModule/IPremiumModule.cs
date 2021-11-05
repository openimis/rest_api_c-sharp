using OpenImis.ModulesV3.PremiumModule.Logic;

namespace OpenImis.ModulesV3.PremiumModule
{
    public interface IPremiumModule
    {
        IPremiumLogic GetPremiumLogic();
        IPremiumModule SetPremiumLogic(IPremiumLogic premiumLogic);
    }
}
