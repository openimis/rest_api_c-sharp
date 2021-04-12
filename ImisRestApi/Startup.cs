using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImisRestApi.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using ImisRestApi.Security;
using OpenImis.Modules.Helpers;
using ImisRestApi.Docs;

namespace ImisRestApi
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //
            //Configuration.Bind(head);
        }

        public IConfiguration Configuration { get; }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configImisModules = Configuration.GetSection("ImisModules").Get<List<ConfigImisModules>>();

            services.AddSingleton<OpenImis.Modules.IImisModules, OpenImis.Modules.ImisModules>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            TokenValidationParameters tokenParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = Configuration["JWT:issuer"],
                ValidAudience = Configuration["JWT:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]))
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(jwtconfig =>
           {
               jwtconfig.TokenValidationParameters = tokenParams;
               // jwtconfig.SecurityTokenValidators.Clear();
               // jwtconfig.SecurityTokenValidators.Add(new TokenValidatorImis());
               jwtconfig.SecurityTokenValidators.Add(new IMISJwtSecurityTokenHandler(
                         services.BuildServiceProvider().GetService<OpenImis.Modules.IImisModules>(),
                         services.BuildServiceProvider().GetService<IHttpContextAccessor>()));
           });
            
            services.AddMvc(config => {
                config.RespectBrowserAcceptHeader = true;
                config.InputFormatters.Add(new XmlSerializerInputFormatter());
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
            //services.ConfigureMvc();
            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Info { Title = "IMIS REST" //, Version = "v1"
                });

                x.DescribeAllEnumsAsStrings();
                x.OperationFilter<FormatXmlCommentProperties>();
                x.OperationFilter<AuthorizationInputOperationFilter>();
                x.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                x.OperationFilter<AddRequiredHeaderParameter>();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            app.UseSwaggerUI(x => {
#if DEBUG
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "IMIS REST");
#else
                x.SwaggerEndpoint("/restapi/swagger/v1/swagger.json", "IMIS REST");
#endif
               // x.SwaggerEndpoint("/swagger/v1/swagger.json", "IMIS REST");
            });
        }
    }
}
