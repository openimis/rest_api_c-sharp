using Microsoft.Extensions.Configuration;
using OpenImis.Modules.SystemModule.Repositories;

namespace OpenImis.Modules.SystemModule.Logic
{
    public class SystemLogic : ISystemLogic
    {
        private IConfiguration _configuration;

        protected ISystemRepository systemRepository;

        public SystemLogic(IConfiguration configuration)
        {
            _configuration = configuration;

            systemRepository = new SystemRepository(_configuration);
        }

        public string Get(string name)
        {
            string response;

            response = systemRepository.Get(name);

            return response;
        }
    }
}
