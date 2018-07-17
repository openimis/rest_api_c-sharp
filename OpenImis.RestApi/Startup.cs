using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenImis.RestApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenImis.RestApi.Security;
using OpenImis.RestApi.Models.Interfaces;
using OpenImis.RestApi.Models.Repository;

using Swashbuckle.AspNetCore.SwaggerGen;
using OpenImis.RestApi.Docs;

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
            // Add our DbContext 
            services.AddDbContext<IMISContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IMISDatabase")));

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
                    options.SecurityTokenValidators.Add(new IMISJwtSecurityTokenHandler(services));
                });

            services.AddMvc();

			services.AddApiVersioning(o => {
				o.ReportApiVersions = true;
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = new ApiVersion(1, 0);
			});

			services.AddSingleton<ICoreRepository, CoreRepository>();

            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (!env.EnvironmentName.Equals("Test"))
            {
                app.UseStaticFiles();
                app.UseSwagger(SwaggerHelper.ConfigureSwagger);
                app.UseSwaggerUI(SwaggerHelper.ConfigureSwaggerUI);
            }

            app.UseAuthentication();
            app.UseMvc();

            
            // ===== Create tables ======
            //imisContext.Database.EnsureCreated();
        }
    }
}
