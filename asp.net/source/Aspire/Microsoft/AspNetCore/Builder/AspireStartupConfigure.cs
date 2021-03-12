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
    using Aspire.Logger;
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
        /// <param name="endpointRouteConfigure">终结点配置.</param>
        /// <param name="swaggerUiName">swagger ui name.</param>
        /// <param name="loggerConfigure">Logger Configure.</param>
        /// <returns>Application Builder .</returns>
        public static IApplicationBuilder UseAspire<TUserEntity>(
            this IApplicationBuilder app,
            Action<IEndpointRouteBuilder> endpointRouteConfigure,
            string swaggerUiName,
            ILoggerConfigure loggerConfigure)
            where TUserEntity : class, IUserEntity, new()
        {
            return UseAspire<TUserEntity, Guid>(
                app,
                endpointRouteConfigure,
                swaggerUiName,
                loggerConfigure);
        }

        /// <summary>
        /// 使用 aspire.
        /// </summary>
        /// <typeparam name="TUserEntity">用户实体.</typeparam>
        /// <typeparam name="TPrimaryKey">实体主键.</typeparam>
        /// <param name="app">Application Builder.</param>
        /// <param name="endpointRouteConfigure">终结点 配置.</param>
        /// <param name="swaggerUiName">swagger ui name.</param>
        /// <param name="loggerConfigure">Logger Configure.</param>
        /// <returns>Application Builder .</returns>
        public static IApplicationBuilder UseAspire<TUserEntity, TPrimaryKey>(
            this IApplicationBuilder app,
            Action<IEndpointRouteBuilder> endpointRouteConfigure,
            string swaggerUiName,
            ILoggerConfigure loggerConfigure)
            where TUserEntity : class, IUserEntity<TPrimaryKey>, new()
        {
            return UseAspire<TUserEntity, TPrimaryKey>(app, actionConfigure =>
            {
                actionConfigure.EndpointRouteConfigure = endpointRouteConfigure;
                actionConfigure.SwaggerUiName = swaggerUiName;
                actionConfigure.LoggerConfigure = loggerConfigure;
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

            if (configure.EndpointRouteConfigure is null)
            {
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.EndpointRouteConfigure));
            }

            if (configure.SwaggerUiName.IsNullOrWhiteSpace())
            {
                throw new NoNullAllowedException(nameof(AspireUseConfigure) + "." + nameof(AspireUseConfigure.SwaggerUiName));
            }

            configure.LoggerConfigure?.UseLogger(app);

            // 初始化 di服务代理 到 静态服务定位类中
            ServiceLocator.Initialize(app.ApplicationServices.GetService<IServiceProviderProxy>());

            // 启用 swagger ui
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", configure.SwaggerUiName));
            }

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