using OpenImis.Modules.PremiumModule.Models;

namespace OpenImis.Modules.PremiumModule.Repositories
{
    public interface IPremiumRepository
    {
        bool Get(ReceiptRequestModel receipt);
    }
}
