using OpenImis.ModulesV3.InsureeModule.Models;

namespace OpenImis.ModulesV3.InsureeModule.Logic
{
    public interface IInsureeLogic
    {
        GetInsureeModel Get(string chfid);
        GetEnquireModel GetEnquire(string chfid);
    }
}
