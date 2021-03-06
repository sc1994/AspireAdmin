using System;
using System.Reflection;
using System.Threading.Tasks;
using Aspire.AutoMapper.Provider;
using Aspire.FreeSql.Provider;
using Aspire.Serilog.ElasticSearch.Provider;
using AspireAdmin.Core.Users;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AspireAdmin.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAspire<User>(options =>
            {
                var applicationAssembly = Assembly.Load("AspireAdmin.Application");

                options.NewtonsoftJsonOptionsSetup = setup =>
                {
                    setup.AllowInputFormatterExceptionMessages = false;
                    setup.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                };

                options.DynamicWebApiOptionsSetup = setup =>
                {
                    // 指定全局默认的 api 前缀
                    setup.DefaultApiPrefix = "api";
                    // 指定程序集 
                    setup.AddAssemblyOptions(applicationAssembly);
                };

                options.SwaggerGenOptionsSetup = setup =>
                {
                    setup.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "AspireAdmin.Host",
                        Version = "v1"
                    });

                    var xmlPath = applicationAssembly.Location.TrimEnd('d', 'l') + "xml";
                    setup.IncludeXmlComments(xmlPath);
                };

                options.MapperOptions = new AutoMapperOptionsSetup(applicationAssembly);

                options.AuditRepositoryOptions = new FreeSqlAuditRepositoryOptionsSetup(this._configuration.GetConnectionString("DbMain"), DataType.Sqlite);

                options.Configuration = this._configuration;

                options.LoggerOptionsSetup = new SerilogElasticSearchOptionsSetup();
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAspire<User>(configure =>
            {
                configure.ServiceProvider = serviceProvider;

                configure.CorsPolicyBuilderConfigure = corsPolicy =>
                {
                    corsPolicy.AllowAnyHeader();
                    corsPolicy.AllowAnyMethod();
                    corsPolicy.AllowCredentials();
                    corsPolicy.WithOrigins(this._configuration.GetSection("WithOrigins").Value.Split(","));
                };

                configure.EndpointRouteConfigure = endpoint =>
                {
                    endpoint.MapControllers();

                    endpoint.Map("/", async cxt =>
                    {
                        await Task.Run(() => cxt.Response.Redirect("/swagger"));
                    });
                };

                configure.SwaggerUiName = "AspireAdmin.Host v1";
            });


        }
    }
}
