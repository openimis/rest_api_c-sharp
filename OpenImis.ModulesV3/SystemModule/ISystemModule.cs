using OpenImis.ModulesV3.SystemModule.Logic;

namespace OpenImis.ModulesV3.SystemModule
{
    public interface ISystemModule
    {
        ISystemLogic GetSystemLogic();
        ISystemModule SetSystemLogic(ISystemLogic systemLogic);
    }
}
