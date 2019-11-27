using OpenImis.ModulesV2.InsureeModule.Models;

namespace OpenImis.ModulesV2.InsureeModule.Repositories
{
    public interface IInsureeRepository
    {
        GetEnquireModel GetEnquire(string chfid);
        GetInsureeModel Get(string chfid);
    }
}
