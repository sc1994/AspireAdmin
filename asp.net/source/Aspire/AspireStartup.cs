using System;
using System.Data;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Aspire;
using Aspire.Core;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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
        public static IServiceCollection AddAspire<TUserEntity>(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
            where TUserEntity : IUserEntity, new()
        {
            return AddAspire<TUserEntity, Guid>(services, setupAction);
        }

        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">选项</param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识</exception>
        /// <returns></returns>
        public static IServiceCollection AddAspire<TUserEntity, TPrimaryKey>(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
            where TUserEntity : IUserEntity<TPrimaryKey>, new()
        {
            var setupOptions = new AspireSetupOptions();
            setupAction(setupOptions);

            // di服务代理 旨在以一个静态类获取 di中内容
            services
                .AddHttpContextAccessor()
                .AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

            var mvcBuilder = services.AddControllers();

            // NewtonsoftJson
            if (setupOptions.NewtonsoftJsonOptionsSetup != null) {
                mvcBuilder.AddNewtonsoftJson(setupOptions.NewtonsoftJsonOptionsSetup);
            }

            // 引入 Panda.DynamicWebApi 自定义配置
            if (setupOptions.DynamicWebApiOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.DynamicWebApiOptionsSetup));
            services.AddDynamicWebApi(optionsAction => {
                setupOptions.DynamicWebApiOptionsSetup(optionsAction);
            });

            // swagger 
            if (setupOptions.SwaggerGenOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.SwaggerGenOptionsSetup));
            services.AddSwaggerGen(x => {
                setupOptions.SwaggerGenOptionsSetup(x);

                // 一定要返回true！这是 Panda.DynamicWebApi 的限制
                x.DocInclusionPredicate((docName, description) => true);
            });

            // mapper
            if (setupOptions.MapperOptions == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.MapperOptions));
            setupOptions.MapperOptions.AddAspireMapper(services);

            // audit repository
            if (setupOptions.AuditRepositoryOptions == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.AuditRepositoryOptions));
            setupOptions.AuditRepositoryOptions.AddAuditRepository(services);

            // aspire configure options
            services.AddScoped(serviceProvider => {
                var aspireConfigureOptions = new AspireConfigureOptions();
                serviceProvider
                     .GetService<IConfiguration>()
                     .GetSection("Aspire")
                     .Bind(aspireConfigureOptions);
                return aspireConfigureOptions;
            });

            // current user 
            services.AddScoped(x => {
                var httpContext = x.GetService<IHttpContextAccessor>().HttpContext;
                var configureOptions = x.GetService<AspireConfigureOptions>();
                if (httpContext != null && httpContext.Request.Headers.TryGetValue(configureOptions.Jwt.HeaderKey, out var token)) {
                    return new JwtManage(x.GetService<AspireConfigureOptions>().Jwt)
                        .DeconstructionJwtToken<TUserEntity, TPrimaryKey>(token.ToString());
                }

                return new TUserEntity {
                    Account = "undefined",
                    Name = "undefined"
                };
            });

            services.AddAuthentication(options => {
                options.AddScheme<AuthenticationHandler>("AuthenticationName", "AuthenticationDisplayName");
                options.DefaultForbidScheme =
                    options.DefaultChallengeScheme =
                        options.DefaultAuthenticateScheme =
                            "AuthenticationName";
            });

            return services;
        }
    }


    internal class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly HttpContext _httpContext;
        private readonly AspireConfigureOptions _aspireConfigureOptions;

        public AuthenticationHandler()
        {
            _httpContext = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext;
            if (_httpContext == null)
                throw new ArgumentNullException(nameof(AuthenticationHandler) + "." + nameof(_httpContext));
            _aspireConfigureOptions = ServiceLocator.ServiceProvider.GetService<AspireConfigureOptions>();
        }

        async public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            //throw new NotImplementedException();
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            if (!_httpContext.Request.Headers.ContainsKey(_aspireConfigureOptions.Jwt.HeaderKey)) {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            if (_httpContext.Request.Headers.TryGetValue(_aspireConfigureOptions.Jwt.HeaderKey, out var token)) {
                if (token != "123") {
                    return AuthenticateResult.Fail("token不正确");
                }
            }

            throw new NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
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
            Action<AspireUseConfigure> actionConfigure)
        {
            var configure = new AspireUseConfigure();
            actionConfigure(configure);
            if (configure.ServiceProvider == null)
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.ServiceProvider));
            if (configure.CorsPolicyBuilderConfigure == null)
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.CorsPolicyBuilderConfigure));
            if (configure.EndpointRouteConfigure == null)
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.EndpointRouteConfigure));
            if (configure.SwaggerUiName.IsNullOrWhiteSpace())
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.SwaggerUiName));

            // 初始化 di服务代理 到 静态服务定位类中
            ServiceLocator.Initialize(configure.ServiceProvider.GetService<IServiceProviderProxy>());

            // 启用 swagger ui
            var env = configure.ServiceProvider.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", configure.SwaggerUiName));
            }

            // 启用跨域
            app.UseCors(configure.CorsPolicyBuilderConfigure);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(configure.EndpointRouteConfigure);


            return app;
        }
    }
}