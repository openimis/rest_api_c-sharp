using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.SystemModule.Logic;

namespace OpenImis.ModulesV3.SystemModule
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
