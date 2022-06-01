using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace OpenImis.DB.SqlServer
{
    public partial class ImisDB : IMISContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "config", Path.DirectorySeparatorChar);
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{path}appsettings.json")
            //.AddJsonFile(Environment.GetEnvironmentVariable("REGISTRY_CONFIG_FILE"))
            //.AddJsonFile("appsettings.json")
            .AddJsonFile(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null ? $"{path}appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json" : $"{path}appsettings.Production.json", optional: false, reloadOnChange: true)
            .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("IMISDatabase"));
        }
    }
}
