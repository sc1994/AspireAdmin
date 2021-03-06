// <copyright file="AspireUseConfigure.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Aspire 配置项.
    /// </summary>
    public class AspireUseConfigure
    {
        /// <summary>
        /// 跨域政策配置
        /// </summary>
        [NotNull]
        public Action<CorsPolicyBuilder> CorsPolicyBuilderConfigure { get; set; }

        /// <summary>
        /// Swagger Ui 名称
        /// </summary>
        [NotNull]
        public string SwaggerUiName { get; set; }

        /// <summary>
        /// 终节点路由配置
        /// </summary>
        [NotNull]
        public Action<IEndpointRouteBuilder> EndpointRouteConfigure { get; set; }

        /// <summary>
        /// di 服务 提供者
        /// </summary>
        [NotNull]
        public IServiceProvider ServiceProvider { get; set; }
    }
}