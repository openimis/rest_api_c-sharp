using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenImis.RestApi.OpenImis.Modules.Utils
{
	public static class Generators
    {
		public static Type CreateTypeFromConfiguration(string moduleName, string sectionName, string defaultValue, ILogger logger = null)
		{

			IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("openImisModules.json", optional: true, reloadOnChange: true)
			.Build();


			Type type;

			string part = "";

			if (configuration.GetSection("ImisModules:" + moduleName).Exists())
			{
				var configSection = configuration.GetSection("ImisModules:" + moduleName);

				if (configSection[sectionName] != null)
				{
					part = configSection[sectionName];
				}
			}

			type = Assembly.GetEntryAssembly().GetType(part);

			if (type == null)
			{
				if (logger != null)
				{
					logger.LogError(moduleName + " " + sectionName + " error: the type " + part + " was not found. Using default " + defaultValue + " configuration.");
				}
				type = Assembly.GetEntryAssembly().GetType(defaultValue);
			}
			else
			{
				if (logger != null)
				{
					logger.LogInformation(moduleName + " load OK: " + part);
				}
			}

			return type;
		}
	}
}
