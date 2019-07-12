using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenImis.RestApi.Docs
{
    internal class SwaggerHelper
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            AddSwaggerDocPerVersion(swaggerGenOptions, webApiAssembly);
            ApplyDocInclusions(swaggerGenOptions);
            
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
                swaggerGenOptions.IncludeXmlComments(xmlFile);

            swaggerGenOptions.DescribeAllEnumsAsStrings();
            swaggerGenOptions.OperationFilter<FormatXmlCommentProperties>();
			swaggerGenOptions.OperationFilter<AuthorizationInputOperationFilter>(); // Adds an Authorization input box to every endpoint
			swaggerGenOptions.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization

            swaggerGenOptions.OperationFilter<AddRequiredHeaderParameter>();
        }

        private static void AddSwaggerDocPerVersion(SwaggerGenOptions swaggerGenOptions, Assembly webApiAssembly)
        {
            var apiVersionDescriptions = new ApiVersionDescriptions();
            apiVersionDescriptions.AddDescription("1", File.ReadAllText("Docs\\ApiVersion1Description.md"));
			var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                swaggerGenOptions.SwaggerDoc($"v{apiVersion}",
                    new Info
                    {
                        Title = "openIMIS REST API",
                        Version = $"v{apiVersion}",
                        Description = apiVersionDescriptions.GetDescription(apiVersion),

                        Contact = new Contact()
                        {
                            Name = "openIMIS",
                            Url = "http://openimis.org"
                        }
                    });
            }
        }

        private static void ApplyDocInclusions(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.DocInclusionPredicate((docName, apiDesc) =>
            {
                var versions = apiDesc.ControllerAttributes()
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v.ToString()}" == docName);
            });
        }

        private static IEnumerable<string> GetApiVersions(Assembly webApiAssembly)
        {
            var apiVersion = webApiAssembly.DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(Controller)) && x.GetCustomAttributes<ApiVersionAttribute>().Any())
                .Select(y => y.GetCustomAttribute<ApiVersionAttribute>())
                .SelectMany(v => v.Versions)
                .Distinct()
                .OrderBy(x => x);

            return apiVersion.Select(x => x.ToString());
        }

        public static void ConfigureSwagger(SwaggerOptions swaggerOptions)
        {
            swaggerOptions.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
			swaggerOptions.RouteTemplate = "api-docs/{documentName}/swagger.json";
			
		}

		public static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                swaggerUIOptions.SwaggerEndpoint($"/api-docs/v{apiVersion}/swagger.json", $"V{apiVersion} Docs");
            }
            swaggerUIOptions.RoutePrefix = "api-docs";
            swaggerUIOptions.InjectStylesheet("theme-feeling-blue-v2.css");
        }
    }
}

