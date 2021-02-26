using System;
using System.Data;
using System.Text;

using Aspire;
using Aspire.Core.Authenticate;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
        public static IServiceCollection AddAspire<TUserEntity, TUserRoleEntity>(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
            where TUserEntity : class, IUserEntity, new()
            where TUserRoleEntity : class, IUserRoleEntity, new()
        {
            return AddAspire<TUserEntity, TUserRoleEntity, Guid>(services, setupAction);
        }

        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">选项</param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识</exception>
        /// <returns></returns>
        public static IServiceCollection AddAspire<TUserEntity, TUserRoleEntity, TPrimaryKey>(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
            where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
            where TUserRoleEntity : class, IUserRoleEntity<TPrimaryKey>, new()
        {
            var options = new AspireSetupOptions();
            setupAction(options);
            if (options.Configuration == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.Configuration));

            var aspireConfigure = GetAspireConfigureOptions(options.Configuration);

            // di服务代理 旨在以一个静态类获取 di中内容
            services
                .AddHttpContextAccessor()
                .AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

            var mvcBuilder = services.AddControllers();

            // NewtonsoftJson
            if (options.NewtonsoftJsonOptionsSetup != null) {
                mvcBuilder.AddNewtonsoftJson(options.NewtonsoftJsonOptionsSetup);
            }

            // 引入 Panda.DynamicWebApi 自定义配置
            if (options.DynamicWebApiOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.DynamicWebApiOptionsSetup));
            services.AddDynamicWebApi(optionsAction => {
                options.DynamicWebApiOptionsSetup(optionsAction);
            });

            // swagger 
            if (options.SwaggerGenOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.SwaggerGenOptionsSetup));
            services.AddSwaggerGen(x => {
                options.SwaggerGenOptionsSetup(x);

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = $"set header: {aspireConfigure.Jwt.HeaderKey}",
                    Name = aspireConfigure.Jwt.HeaderKey, // 自定义 header key
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                // 一定要返回true！这是 Panda.DynamicWebApi 的限制
                x.DocInclusionPredicate((docName, description) => true);
            });

            // mapper
            if (options.MapperOptions == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.MapperOptions));
            options.MapperOptions.AddAspireMapper(services);

            // audit repository
            if (options.AuditRepositoryOptions == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.AuditRepositoryOptions));
            options.AuditRepositoryOptions.AddAuditRepository(services);

            // aspire configure options
            services.AddScoped(serviceProvider => GetAspireConfigureOptions(serviceProvider.GetService<IConfiguration>()));

            // TODO current user 
            //services.AddScoped(x => {
            //    var httpContext = x.GetService<IHttpContextAccessor>().HttpContext;
            //    var configureOptions = x.GetService<AspireConfigureOptions>();
            //    if (httpContext != null && httpContext.Request.Headers.TryGetValue(configureOptions.Jwt.HeaderKey, out var token)) {
            //        return new JwtManage(x.GetService<AspireConfigureOptions>().Jwt)
            //            .DeconstructionJwtToken<TUserEntity, TPrimaryKey>(token.ToString());
            //    }

            //    return new TUserEntity {
            //        Account = "undefined",
            //        Name = "undefined"
            //    };
            //});

            services.AddIdentity<TUserEntity, TUserRoleEntity>()
                .AddUserStore<UserStore<TUserEntity, TPrimaryKey>>()
                .AddRoleStore<RoleStore<TUserRoleEntity, TPrimaryKey>>();

            services
                .AddAuthentication(x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x => {
                    x.SaveToken = true;
#if DEBUG
                    x.RequireHttpsMetadata = false;
#endif
                    x.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = aspireConfigure.Jwt.ValidAudience,
                        ValidIssuer = aspireConfigure.Jwt.ValidIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(aspireConfigure.Jwt.Secret))
                    };
                });

            return services;
        }

        private static AspireConfigureOptions GetAspireConfigureOptions(IConfiguration configuration)
        {
            var aspireConfigureOptions = new AspireConfigureOptions();
            configuration.GetSection("Aspire")
                .Bind(aspireConfigureOptions);
            return aspireConfigureOptions;
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