using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ImisRestApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ImisRestApi.Security;
using ImisRestApi.Models.Interfaces;
using ImisRestApi.Models.Repository;

namespace ImisRestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            services.AddSingleton<IIMISRepository, IMISRepository>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMISContext imisContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
        
            // ===== Create tables ======
            //imisContext.Database.EnsureCreated();
        }
    }
}
