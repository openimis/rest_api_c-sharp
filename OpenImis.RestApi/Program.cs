using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.Extensions.Configuration;

namespace OpenImis.RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appRootPath = Directory.GetCurrentDirectory();
            BuildWebHost(appRootPath, args).Run();
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();


        public static IWebHost BuildWebHost(string appRootPath, string[] args)
        {
            var webHostBuilder = GetWebHostBuilder(appRootPath, args);
            return webHostBuilder.Build();
        }


        public static IWebHostBuilder GetWebHostBuilder(string appRootPath, string[] args)
        {
            var webHostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(appRootPath)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //var secretsMode = GetSecretsMode(hostingContext.HostingEnvironment);
                    //config.AddOpenImisConfig(secretsMode, "REGISTRY_CONFIG_FILE");
					//config.AddOpenImisConfig(SecretsMode.LocalFile, $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
					config.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
#if DEBUG
                    config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
#else
                    config.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
#endif
#if CHF
                    config.AddJsonFile("appsettings.CHF.json", optional: false, reloadOnChange: true);
#endif
                    config.AddJsonFile("openImisModules.json", optional: true, reloadOnChange: true);
				})
                .UseStartup<Startup>();

            return webHostBuilder;
        }

        private static SecretsMode GetSecretsMode(IHostingEnvironment env)
        {
            if (env.IsProduction())
                return SecretsMode.DockerSecrets;

            var useDockerSecrets = Environment.GetEnvironmentVariable("REGISTRY_USE_DOCKER_SECRETS");
            if (useDockerSecrets != null && useDockerSecrets.Equals("false", StringComparison.OrdinalIgnoreCase))
                return SecretsMode.LocalFile;

            return SecretsMode.DockerSecrets;
        }
    }
}
