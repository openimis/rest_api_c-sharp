using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.SystemModule.Repositories;

namespace OpenImis.ModulesV3.SystemModule.Logic
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
