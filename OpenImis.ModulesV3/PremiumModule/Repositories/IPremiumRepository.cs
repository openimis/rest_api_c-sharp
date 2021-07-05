using OpenImis.ModulesV3.PremiumModule.Models;

namespace OpenImis.ModulesV3.PremiumModule.Repositories
{
    public interface IPremiumRepository
    {
        bool Get(ReceiptRequestModel receipt);
    }
}
