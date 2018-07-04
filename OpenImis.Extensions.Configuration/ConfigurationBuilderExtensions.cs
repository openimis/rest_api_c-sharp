using Microsoft.Extensions.Configuration;
using System.Linq;

namespace OpenImis.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddOpenImisConfig(this IConfigurationBuilder builder, SecretsMode secretsMode, params string[] pathEnvironmentVariables)
        {
            var settingsConfigSource = new OpenImisConfigurationSource(secretsMode, pathEnvironmentVariables.ToList());
            return builder.Add(settingsConfigSource);
        }
    }
}
