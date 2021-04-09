using OpenImis.Modules.InsureeModule.Models;

namespace OpenImis.Modules.InsureeModule.Repositories
{
    public interface IInsureeRepository
    {
        GetEnquireModel GetEnquire(string chfid);
        GetInsureeModel Get(string chfid);
    }
}
