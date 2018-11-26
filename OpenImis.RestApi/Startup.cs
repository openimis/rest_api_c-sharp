using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenImis.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenImis.RestApi.Security;
using Swashbuckle.AspNetCore.SwaggerGen;
using OpenImis.RestApi.Docs;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Logging;

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
			// Add the DbContext 
			//services.AddDbContext<IMISContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IMISDatabase")));

			services.AddSingleton<IImisModules, ImisModules>();

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
                    options.SecurityTokenValidators.Add(new IMISJwtSecurityTokenHandler(services.BuildServiceProvider().GetService<IImisModules>()));
                });

            services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddApiVersioning(o => {
				o.ReportApiVersions = true;
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = new ApiVersion(1, 0);
				o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new HeaderApiVersionReader("api-version"));
			});

			
            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

			services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin",
					builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader());
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
