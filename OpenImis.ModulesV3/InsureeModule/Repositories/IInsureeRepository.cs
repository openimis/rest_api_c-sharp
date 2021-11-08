using OpenImis.ModulesV3.InsureeModule.Models;

namespace OpenImis.ModulesV3.InsureeModule.Repositories
{
    public interface IInsureeRepository
    {
        GetEnquireModel GetEnquire(string chfid);
        GetInsureeModel Get(string chfid);
    }
}
