// ReSharper disable once CheckNamespace

using System;
using System.Reflection;

using Aspire;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Panda.DynamicWebApi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AspireStartup
    {
        public static IServiceCollection AddAspire(
            this IServiceCollection services,
            Assembly applicationAssembly)
        {
            // di服务代理 旨在 以一个静态类获取 di中内容
            services.AddHttpContextAccessor();
            services.AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

            services.AddControllers().AddNewtonsoftJson();

            // 引入 Panda.DynamicWebApi 自定义配置
            services.AddDynamicWebApi(options => {

                // 指定全局默认的 api 前缀
                options.DefaultApiPrefix = "api";

                // 指定程序集 
                options.AddAssemblyOptions(applicationAssembly);
            });

            // swagger 
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AspireAdmin.Host", Version = "v1" });

                // 一定要返回true！这是 Panda.DynamicWebApi 的限制
                options.DocInclusionPredicate((docName, description) => true);

                var xmlPath = applicationAssembly.Location.TrimEnd('d', 'l') + "xml";
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}


// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class AspireStartup
    {
        public static IApplicationBuilder UseAspire(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider)
        {
            // 初始化 di服务代理 到 静态服务定位类中
            ServiceLocator.Initialize(serviceProvider.GetService<IServiceProviderProxy>());

            // 启用 swagger ui
            var env = serviceProvider.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspireAdmin.Host v1"));
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}