using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenImis.RestApi.Security;
using OpenImis.RestApi.Docs;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Logging;
using OpenImis.RestApi.Controllers;
using OpenImis.ModulesV1;
using OpenImis.ModulesV2;
using Microsoft.AspNetCore.Http;
using OpenImis.ModulesV1.Helpers;
using System.Collections.Generic;

namespace OpenImis.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var configImisModules = Configuration.GetSection("ImisModules").Get<List<ConfigImisModules>>();
            int lastApiVersion = int.Parse(configImisModules[configImisModules.Count - 1].Version);

            services.AddSingleton<ModulesV1.IImisModules, ModulesV1.ImisModules>();
            services.AddSingleton<ModulesV2.IImisModules, ModulesV2.ImisModules>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add the authentication scheme with the custom validator
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"]
                    };

                    options.SecurityTokenValidators.Clear();
                    //below line adds the custom validator class
                    options.SecurityTokenValidators.Add(new IMISJwtSecurityTokenHandler(
                        services.BuildServiceProvider().GetService<ModulesV1.IImisModules>(),
                        services.BuildServiceProvider().GetService<ModulesV2.IImisModules>(),
                        services.BuildServiceProvider().GetService<IHttpContextAccessor>(),
                        lastApiVersion));
                });

            services.AddAuthorization();

            services.AddMvc(options =>
            {
                options.AllowCombiningAuthorizeFilters = false;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddControllersAsServices();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(lastApiVersion, 0);
                o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new HeaderApiVersionReader("api-version"));
            });


            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }
            if (!env.EnvironmentName.Equals("Test"))
            {
                app.UseStaticFiles();
                app.UseSwagger(SwaggerHelper.ConfigureSwagger);
                app.UseSwaggerUI(SwaggerHelper.ConfigureSwaggerUI);
            }

            app.UseAuthentication();
            app.UseMvc();

            app.UseCors("AllowSpecificOrigin");


            // ===== Create tables ======
            //imisContext.Database.EnsureCreated();
        }
    }
}
