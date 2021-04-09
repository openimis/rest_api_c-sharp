using Microsoft.Extensions.Configuration;
using OpenImis.Modules.SystemModule.Logic;

namespace OpenImis.Modules.SystemModule
{
    public class SystemModule : ISystemModule
    {
        private IConfiguration _configuration;
        private ISystemLogic _systemLogic;

        public SystemModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ISystemLogic GetSystemLogic()
        {
            if (_systemLogic == null)
            {
                _systemLogic = new SystemLogic(_configuration);
            }
            return _systemLogic;
        }

        public ISystemModule SetSystemLogic(ISystemLogic systemLogic)
        {
            _systemLogic = systemLogic;
            return this;
        }
    }
}
