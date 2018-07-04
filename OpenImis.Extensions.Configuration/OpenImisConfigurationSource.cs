using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace OpenImis.Extensions.Configuration
{
    public class OpenImisConfigurationSource : IConfigurationSource
    {
        private SecretsMode _secretsMode;
        private List<string> _pathEnvironmentVariables;

        public OpenImisConfigurationSource(SecretsMode secretsMode,
            List<string> pathEnvironmentVariables)
        {
            _secretsMode = secretsMode;
            _pathEnvironmentVariables = pathEnvironmentVariables;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new OpenImisConfigurationProvider(
                _secretsMode,
                _pathEnvironmentVariables);
        }
    }
}
