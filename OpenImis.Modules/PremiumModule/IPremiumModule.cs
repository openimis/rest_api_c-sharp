using OpenImis.Modules.PremiumModule.Logic;

namespace OpenImis.Modules.PremiumModule
{
    public interface IPremiumModule
    {
        IPremiumLogic GetPremiumLogic();
        IPremiumModule SetPremiumLogic(IPremiumLogic premiumLogic);
    }
}
