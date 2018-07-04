using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace OpenImis.RestApi.IntegrationTests.Helpers
{
    class TestFixture
    {
        public TestServer Server { get; set; }
        public HttpClient Client { get; set; }

        public TestFixture()
        {
            // We must configure the realpath of the targeted project
            string appRootPath = Path.GetFullPath(Path.Combine(
                            AppContext.BaseDirectory,
                            "..", "..", "..", "..", "OpenImis.RestApi"));

            // set environment variables the application needs to read on start up
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            Environment.SetEnvironmentVariable("REGISTRY_CONFIG_FILE", Path.Combine(appRootPath, "appsettings.json"));
            //Environment.SetEnvironmentVariable("REGISTRY_DB_PASSWORD_SECRET_FILE", Path.Combine(appRootPath, "InsecureSecretFiles", "RegistryDbPassword.txt"));
            Environment.SetEnvironmentVariable("REGISTRY_USE_DOCKER_SECRETS", "false");

            Server = new TestServer(Program.GetWebHostBuilder(appRootPath, null));
            var client = Server.CreateClient();

            Client = client;
        }
    }
}
