using System;
using System.Data;

using Aspire;
using Aspire.Core.UserInfos;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Panda.DynamicWebApi;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// aspire 启动
    /// </summary>
    public static class AspireStartup
    {
        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">选项</param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识</exception>
        /// <returns></returns>
        public static IServiceCollection AddAspire(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
        {
            var setupOptions = new AspireSetupOptions();
            setupAction(setupOptions);

            // di服务代理 旨在 以一个静态类获取 di中内容
            services
                .AddHttpContextAccessor()
                .AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

            var mvcBuilder = services.AddControllers();

            // NewtonsoftJson
            if (setupOptions.NewtonsoftJsonOptionsSetup != null) {
                mvcBuilder.AddNewtonsoftJson(setupOptions.NewtonsoftJsonOptionsSetup);
            }

            // 引入 Panda.DynamicWebApi 自定义配置
            services.AddDynamicWebApi(optionsAction => {
                if (setupOptions.DynamicWebApiOptionsSetup == null)
                    throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.DynamicWebApiOptionsSetup));
                setupOptions.DynamicWebApiOptionsSetup(optionsAction);
            });

            // swagger 
            services.AddSwaggerGen(x => {
                if (setupOptions.SwaggerGenOptionsSetup == null)
                    throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.SwaggerGenOptionsSetup));

                // 一定要返回true！这是 Panda.DynamicWebApi 的限制
                x.DocInclusionPredicate((docName, description) => true);
            });

            // mapper
            if (setupOptions.MapperOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.MapperOptionsSetup));
            setupOptions.MapperOptionsSetup.AddAspireMapper(services);

            // audit repository
            if (setupOptions.AuditRepositoryOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.AuditRepositoryOptionsSetup));
            setupOptions.AuditRepositoryOptionsSetup.AddAuditRepository(services);

            // user login info 
            services.AddScoped<ICurrentLoginUser>(x => new TestUser());

            return services;
        }
    }
}


// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// aspire 启动
    /// </summary>
    public static class AspireStartup
    {
        /// <summary>
        /// 使用 aspire
        /// </summary>
        /// <param name="app"></param>
        /// <param name="actionConfigure"></param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识</exception>
        /// <returns></returns>
        public static IApplicationBuilder UseAspire(
            this IApplicationBuilder app,
            Action<AspireConfigure> actionConfigure)
        {
            var configure = new AspireConfigure();
            actionConfigure(configure);
            if (configure.ServiceProvider == null)
                throw new NoNullAllowedException(nameof(AspireConfigure) + "." + nameof(AspireConfigure.ServiceProvider));
            if (configure.CorsPolicyBuilderConfigure == null)
                throw new NoNullAllowedException(nameof(AspireConfigure) + "." + nameof(AspireConfigure.CorsPolicyBuilderConfigure));
            if (configure.EndpointRouteConfigure == null)
                throw new NoNullAllowedException(nameof(AspireConfigure) + "." + nameof(AspireConfigure.EndpointRouteConfigure));
            if (configure.SwaggerUiName.IsNullOrWhiteSpace())
                throw new NoNullAllowedException(nameof(AspireConfigure) + "." + nameof(AspireConfigure.SwaggerUiName));

            // 初始化 di服务代理 到 静态服务定位类中
            ServiceLocator.Initialize(configure.ServiceProvider.GetService<IServiceProviderProxy>());

            // 启用 swagger ui
            var env = configure.ServiceProvider.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", configure.SwaggerUiName));
            }

            app.UseRouting();
            app.UseEndpoints(configure.EndpointRouteConfigure);

            app.UseCors(configure.CorsPolicyBuilderConfigure);

            return app;
        }
    }
}