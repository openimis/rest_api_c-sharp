using OpenImis.ModulesV2.SystemModule.Logic;

namespace OpenImis.ModulesV2.SystemModule
{
    public interface ISystemModule
    {
        ISystemLogic GetSystemLogic();
        ISystemModule SetSystemLogic(ISystemLogic systemLogic);
    }
}
