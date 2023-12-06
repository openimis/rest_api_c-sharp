using System;
using System.Data;
using System.Data.Common;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OpenImis.DB.SqlServer
{
    public partial class ImisDB : IMISContext
    {
        private int _commandTimeout = 30;

        public DbCommand CreateCommand()
        {
            var connection = Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandTimeout = _commandTimeout;
            return command;
        }

        public void CheckConnection()
        {
            var connection = Database.GetDbConnection();
            if (connection.State.Equals(ConnectionState.Closed)) connection.Open();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "config", Path.DirectorySeparatorChar);
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{path}appsettings.json")
            //.AddJsonFile(Environment.GetEnvironmentVariable("REGISTRY_CONFIG_FILE"))
            //.AddJsonFile("appsettings.json")
            .AddJsonFile(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null ? $"{path}appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json" : $"{path}appsettings.Production.json", optional: false, reloadOnChange: false)
            .Build();
            if (configuration["ConnectionSettings:CommandTimeout"] != null) _commandTimeout = Int32.Parse(configuration["ConnectionSettings:CommandTimeout"]);
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("IMISDatabase"), options => options.CommandTimeout(_commandTimeout));
        }
    }
}
