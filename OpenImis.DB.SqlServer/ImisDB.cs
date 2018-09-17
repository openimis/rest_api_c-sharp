using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace OpenImis.DB.SqlServer
{
    public partial class ImisDB:IMISContext
    {

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile(Environment.GetEnvironmentVariable("REGISTRY_CONFIG_FILE"))
			//.AddJsonFile("appsettings.json")
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
			.Build();

			optionsBuilder.UseSqlServer(configuration.GetConnectionString("IMISDatabase"));
		}
	}
}
