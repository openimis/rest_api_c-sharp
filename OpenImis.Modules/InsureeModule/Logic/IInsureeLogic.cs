using OpenImis.Modules.InsureeModule.Models;

namespace OpenImis.Modules.InsureeModule.Logic
{
    public interface IInsureeLogic
    {
        GetInsureeModel Get(string chfid);
        GetEnquireModel GetEnquire(string chfid);
    }
}
