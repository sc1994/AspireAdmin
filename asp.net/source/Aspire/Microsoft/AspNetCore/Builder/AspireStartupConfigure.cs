// <copyright file="AspireStartupConfigure.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    using System;
    using System.Data;
    using Aspire;
    using Aspire.Authenticate;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// aspire 启动.
    /// </summary>
    public static class AspireStartupConfigure
    {
        /// <summary>
        /// 使用 aspire.
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体.</typeparam>
        /// <param name="app">Application Builder.</param>
        /// <param name="serviceProvider">提供服务.</param>
        /// <param name="endpointRouteConfigure">终结点【配置.</param>
        /// <param name="swaggerUiName">swagger ui name.</param>
        /// <param name="corsPolicyBuilderConfigure">跨域代理配置.</param>
        /// <returns>Application Builder .</returns>
        public static IApplicationBuilder UseAspire<TUserEntity>(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider,
            Action<IEndpointRouteBuilder> endpointRouteConfigure,
            string swaggerUiName,
            Action<CorsPolicyBuilder> corsPolicyBuilderConfigure)
            where TUserEntity : class, IUserEntity, new()
        {
            return UseAspire<TUserEntity, Guid>(
                app,
                serviceProvider,
                endpointRouteConfigure,
                swaggerUiName,
                corsPolicyBuilderConfigure);
        }

        /// <summary>
        /// 使用 aspire.
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体.</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
        /// <param name="app">Application Builder.</param>
        /// <param name="serviceProvider">提供服务.</param>
        /// <param name="endpointRouteConfigure">终结点 配置.</param>
        /// <param name="swaggerUiName">swagger ui name.</param>
        /// <param name="corsPolicyBuilderConfigure">跨域代理配置.</param>
        /// <returns>Application Builder .</returns>
        public static IApplicationBuilder UseAspire<TUserEntity, TPrimaryKey>(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider,
            Action<IEndpointRouteBuilder> endpointRouteConfigure,
            string swaggerUiName,
            Action<CorsPolicyBuilder> corsPolicyBuilderConfigure)
            where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
        {
            return UseAspire<TUserEntity, TPrimaryKey>(app, actionConfigure =>
            {
                actionConfigure.ServiceProvider = serviceProvider;
                actionConfigure.EndpointRouteConfigure = endpointRouteConfigure;
                actionConfigure.SwaggerUiName = swaggerUiName;
                actionConfigure.CorsPolicyBuilderConfigure = corsPolicyBuilderConfigure;
            });
        }

        /// <summary>
        /// 使用 aspire.
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体.</typeparam>
        /// <param name="app">Application Builder.</param>
        /// <param name="actionConfigure">Aspire Use Configure.</param>
        /// <exception cref="ArgumentNullException">请注意 [NotNull] 标识.</exception>
        /// <returns>Application Builder .</returns>
        public static IApplicationBuilder UseAspire<TUserEntity>(
            this IApplicationBuilder app,
            Action<AspireUseConfigure> actionConfigure)
            where TUserEntity : class, IUserEntity, new()
        {
            return UseAspire<TUserEntity, Guid>(app, actionConfigure);
        }

        /// <summary>
        /// 使用 aspire.
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体.</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
        /// <param name="app">Application Builder.</param>
        /// <param name="actionConfigure">请注意 [NotNull] 标识.</param>
        /// <returns>Application Builder .</returns>
        public static IApplicationBuilder UseAspire<TUserEntity, TPrimaryKey>(
            this IApplicationBuilder app,
            Action<AspireUseConfigure> actionConfigure)
            where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
        {
            var configure = new AspireUseConfigure();
            actionConfigure(configure);
            if (configure.ServiceProvider == null)
            {
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.ServiceProvider));
            }

            if (configure.CorsPolicyBuilderConfigure == null)
            {
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.CorsPolicyBuilderConfigure));
            }

            if (configure.EndpointRouteConfigure == null)
            {
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.EndpointRouteConfigure));
            }

            if (configure.SwaggerUiName.IsNullOrWhiteSpace())
            {
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.SwaggerUiName));
            }

            // 初始化 di服务代理 到 静态服务定位类中
            ServiceLocator.Initialize(configure.ServiceProvider.GetService<IServiceProviderProxy>());

            // 启用 swagger ui
            var env = configure.ServiceProvider.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", configure.SwaggerUiName));
            }

            // 启用跨域
            app.UseCors(configure.CorsPolicyBuilderConfigure);

            app.UseRouting();

            /*
             暂时放弃 asp.net identity 方案，实现过于繁琐
             app.UseAuthentication();
             app.UseAuthorization();
            */

            app.UseMiddleware<JwtMiddleware<TUserEntity>>();

            app.UseEndpoints(configure.EndpointRouteConfigure);

            return app;
        }
    }
}