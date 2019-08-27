using OpenImis.ModulesV2.InsureeModule.Models;

namespace OpenImis.ModulesV2.InsureeModule.Repositories
{
    public interface IInsureeRepository
    {
        GetInsureeModel Get(string chfid);
    }
}
