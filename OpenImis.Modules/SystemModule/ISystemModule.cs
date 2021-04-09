using OpenImis.Modules.SystemModule.Logic;

namespace OpenImis.Modules.SystemModule
{
    public interface ISystemModule
    {
        ISystemLogic GetSystemLogic();
        ISystemModule SetSystemLogic(ISystemLogic systemLogic);
    }
}
