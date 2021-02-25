using System;
using System.Reflection;
using System.Threading.Tasks;

using Aspire.AutoMapper.Provider;
using Aspire.FreeSql.Provider;

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
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAspire<User>(options => {
                var applicationAssembly = Assembly.Load("AspireAdmin.Application");

                options.NewtonsoftJsonOptionsSetup = setup => {
                    setup.AllowInputFormatterExceptionMessages = false;
                };

                options.DynamicWebApiOptionsSetup = setup => {
                    // 指定全局默认的 api 前缀
                    setup.DefaultApiPrefix = "api";
                    // 指定程序集 
                    setup.AddAssemblyOptions(applicationAssembly);
                };

                options.SwaggerGenOptionsSetup = setup => {
                    setup.SwaggerDoc("v1", new OpenApiInfo {
                        Title = "AspireAdmin.Host",
                        Version = "v1"
                    });

                    var headerKey = _configuration.GetSection("Aspire").GetSection("Jwt").GetSection("HeaderKey").Value;
                    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                        Description = $"set header: {headerKey}",
                        Name = headerKey, // 自定义 header key
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                    });

                    var xmlPath = applicationAssembly.Location.TrimEnd('d', 'l') + "xml";
                    setup.IncludeXmlComments(xmlPath);
                };

                options.MapperOptions = new AutoMapperOptionsSetup(applicationAssembly);

                options.AuditRepositoryOptions = new FreeSqlAuditRepositoryOptionsSetup(_configuration.GetConnectionString("DbMain"), DataType.Sqlite);
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseAspire(configure => {
                configure.ServiceProvider = serviceProvider;

                configure.CorsPolicyBuilderConfigure = corsPolicy => {
                    corsPolicy.AllowAnyHeader();
                    corsPolicy.AllowAnyMethod();
                    corsPolicy.AllowCredentials();
                    corsPolicy.WithOrigins(_configuration.GetSection("WithOrigins").Value.Split(","));
                };

                configure.EndpointRouteConfigure = endpoint => {
                    endpoint.MapControllers();

                    endpoint.Map("/", async cxt => {
                        await Task.Run(() => cxt.Response.Redirect("/swagger"));
                    });
                };

                configure.SwaggerUiName = "AspireAdmin.Host v1";
            });


        }
    }
}
