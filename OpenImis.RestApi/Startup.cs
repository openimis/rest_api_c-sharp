using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenImis.Security.Security;
using OpenImis.RestApi.Docs;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using OpenImis.ModulesV1.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using System;
using OpenImis.ePayment.Scheduler;
using OpenImis.RestApi.Scheduler;
using OpenImis.RestApi.Util.ErrorHandling;
// using OpenImis.ePayment.Formaters;

namespace OpenImis.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            LoggerFactory = loggerFactory;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; }

        private ILoggerFactory LoggerFactory { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var configImisModules = Configuration.GetSection("ImisModules").Get<List<ConfigImisModules>>();
            int lastApiVersion = 3; // configImisModules.Max(c => int.Parse(c.Version));

            services.AddSingleton<ModulesV1.IImisModules, ModulesV1.ImisModules>();
            services.AddSingleton<ModulesV2.IImisModules, ModulesV2.ImisModules>();
            services.AddSingleton<ModulesV3.IImisModules, ModulesV3.ImisModules>();
            services.AddSingleton<Security.ILoginModule, Security.LoginModule>();

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
                    //options.SecurityTokenValidators.Add(new IMISJwtSecurityTokenHandler(
                    //    services.BuildServiceProvider().GetService<ModulesV1.IImisModules>(),
                    //    services.BuildServiceProvider().GetService<ModulesV2.IImisModules>(),
                    //    services.BuildServiceProvider().GetService<ModulesV3.IImisModules>(),
                    //    services.BuildServiceProvider().GetService<IHttpContextAccessor>()));

                    options.SecurityTokenValidators.Add(new IMISJwtSecurityTokenHandler(
                        services.BuildServiceProvider().GetService<IHttpContextAccessor>()));

                });

            services.AddAuthorization();

            services.AddMvc(options =>
            {
                options.AllowCombiningAuthorizeFilters = false;
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
#if CHF
                options.InputFormatters.Add(new ePayment.Formaters.GePGXmlSerializerInputFormatter(HostingEnvironment, Configuration, LoggerFactory));
#else
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
#endif
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
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


            // Quartz scheduler
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            services.AddSingleton(scheduler);
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<IJobFactory, CustomQuartzJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<MatchPaymentJob>();

            var jobName = Configuration.GetSection("MatchPaymentSchedule").GetValue<string>("JobName");
            var cronExpression = Configuration.GetSection("MatchPaymentSchedule").GetValue<string>("CronExpression");

            services.AddSingleton(new JobMetaData(Guid.NewGuid(), typeof(MatchPaymentJob), jobName , cronExpression));
            services.AddHostedService<CustomQuartzHostedService>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggingOptions = Configuration.GetSection("Log4NetCore").Get<Log4NetProviderOptions>();
            loggerFactory.AddLog4Net(loggingOptions);

            app.UseMiddleware<ErrorHandlerMiddleware>();

            if (env.IsDevelopment())
            {
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
