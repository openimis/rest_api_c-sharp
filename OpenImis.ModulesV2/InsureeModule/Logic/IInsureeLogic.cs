using OpenImis.ModulesV2.InsureeModule.Models;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public interface IInsureeLogic
    {
        GetInsureeModel Get(string chfid);
    }
}
