using System;
using System.Data;

using Aspire;
using Aspire.Authenticate;
using Aspire.Mapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Panda.DynamicWebApi;

using Swashbuckle.AspNetCore.SwaggerGen;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// aspire 启动
    /// </summary>
    public static class AspireConfigureServices
    {
        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体</typeparam>
        /// <param name="services"></param>
        /// <param name="dynamicWebApiOptionsSetup">动态 api 启动选项</param>
        /// <param name="swaggerGenOptionsSetup">swagger 启动选项</param>
        /// <param name="mapperOptions">Mapper 设置项</param>
        /// <param name="auditRepositoryOptions">审计仓储 设置项</param>
        /// <param name="configuration">配置</param>
        /// <param name="newtonsoftJsonOptionsSetup">NewtonsoftJson 启动选项</param>
        /// <returns></returns>
        public static IServiceCollection AddAspire<TUserEntity>(
            this IServiceCollection services,
            Action<DynamicWebApiOptions> dynamicWebApiOptionsSetup,
            Action<SwaggerGenOptions> swaggerGenOptionsSetup,
            IAspireMapperOptionsSetup mapperOptions,
            IAuditRepositoryOptionsSetup auditRepositoryOptions,
            IConfiguration configuration,
            Action<MvcNewtonsoftJsonOptions> newtonsoftJsonOptionsSetup = null)
            where TUserEntity : class, IUserEntity, new()
        {
            return AddAspire<TUserEntity>(services, setupAction => {
                setupAction.Configuration = configuration;
                setupAction.SwaggerGenOptionsSetup = swaggerGenOptionsSetup;
                setupAction.DynamicWebApiOptionsSetup = dynamicWebApiOptionsSetup;
                setupAction.AuditRepositoryOptions = auditRepositoryOptions;
                setupAction.MapperOptions = mapperOptions;
                setupAction.NewtonsoftJsonOptionsSetup = newtonsoftJsonOptionsSetup;
            });
        }

        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键</typeparam>
        /// <param name="services"></param>
        /// <param name="dynamicWebApiOptionsSetup">动态 api 启动选项</param>
        /// <param name="swaggerGenOptionsSetup">swagger 启动选项</param>
        /// <param name="mapperOptions">Mapper 设置项</param>
        /// <param name="auditRepositoryOptions">审计仓储 设置项</param>
        /// <param name="configuration">配置</param>
        /// <param name="newtonsoftJsonOptionsSetup">NewtonsoftJson 启动选项</param>
        /// <returns></returns>
        public static IServiceCollection AddAspire<TUserEntity, TPrimaryKey>(
            this IServiceCollection services,
            Action<DynamicWebApiOptions> dynamicWebApiOptionsSetup,
            Action<SwaggerGenOptions> swaggerGenOptionsSetup,
            IAspireMapperOptionsSetup mapperOptions,
            IAuditRepositoryOptionsSetup auditRepositoryOptions,
            IConfiguration configuration,
            Action<MvcNewtonsoftJsonOptions> newtonsoftJsonOptionsSetup = null)
            where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
        {
            return AddAspire<TUserEntity, TPrimaryKey>(services, setupAction => {
                setupAction.Configuration = configuration;
                setupAction.SwaggerGenOptionsSetup = swaggerGenOptionsSetup;
                setupAction.DynamicWebApiOptionsSetup = dynamicWebApiOptionsSetup;
                setupAction.AuditRepositoryOptions = auditRepositoryOptions;
                setupAction.MapperOptions = mapperOptions;
                setupAction.NewtonsoftJsonOptionsSetup = newtonsoftJsonOptionsSetup;
            });
        }

        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体</typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction">选项</param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识</exception>
        /// <returns></returns>
        public static IServiceCollection AddAspire<TUserEntity>(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
            where TUserEntity : class, IUserEntity, new()
        {
            return AddAspire<TUserEntity, Guid>(services, setupAction);
        }

        /// <summary>
        /// 添加 aspire
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键</typeparam>
        /// <param name="services"></param>
        /// <param name="setupAction">选项</param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识</exception>
        /// <returns></returns>
        public static IServiceCollection AddAspire<TUserEntity, TPrimaryKey>(
            this IServiceCollection services,
            Action<AspireSetupOptions> setupAction)
            where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
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
            services.Configure<AspireAppSettings>(options.Configuration.GetSection("Aspire"));

            // current user 
            services.AddScoped(x => {
                var httpContext = x.GetService<IHttpContextAccessor>().HttpContext;
                var configureOptions = x.GetService<IOptions<AspireAppSettings>>().Value;
                if (httpContext != null && httpContext.Request.Headers.TryGetValue(configureOptions.Jwt.HeaderKey, out var token)) {
                    return new JwtManage(configureOptions.Jwt)
                        .DeconstructionJwtToken<TUserEntity>(token.ToString());
                }

                return new TUserEntity {
                    Account = "undefined",
                    Name = "undefined"
                };
            });

            #region 暂时放弃 asp.net identity 方案，实现过于繁琐
            //services.AddIdentity<TUserEntity, TUserRoleEntity>()
            //.AddEntityFrameworkStores<AspireIdentityDbContext<TUserEntity>>()
            //.AddUserStore<>()
            //.AddRoleStore<>()
            //.AddDefaultTokenProviders();
            //services
            //    .AddAuthentication(x => {
            //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(x => {
            //        x.SaveToken = true;
            //        x.RequireHttpsMetadata = false;
            //        x.TokenValidationParameters = new TokenValidationParameters {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidAudience = aspireConfigure.Jwt.ValidAudience,
            //            ValidIssuer = aspireConfigure.Jwt.ValidIssuer,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(aspireConfigure.Jwt.Secret))
            //        };
            //    });
            #endregion

            // 引入 Panda.DynamicWebApi 自定义配置
            if (options.DynamicWebApiOptionsSetup == null)
                throw new NoNullAllowedException(nameof(AspireSetupOptions) + "." + nameof(AspireSetupOptions.DynamicWebApiOptionsSetup));

            services.AddDynamicWebApi(optionsAction => {
                options.DynamicWebApiOptionsSetup(optionsAction);
            });

            return services;
        }

        private static AspireAppSettings GetAspireConfigureOptions(IConfiguration configuration)
        {
            var aspireConfigureOptions = new AspireAppSettings();
            configuration.GetSection("Aspire")
                .Bind(aspireConfigureOptions);
            return aspireConfigureOptions;
        }
    }
}
