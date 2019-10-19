using OpenImis.ModulesV2.PremiumModule.Models;

namespace OpenImis.ModulesV2.PremiumModule.Repositories
{
    public interface IPremiumRepository
    {
        bool Get(ReceiptRequestModel receipt);
    }
}
