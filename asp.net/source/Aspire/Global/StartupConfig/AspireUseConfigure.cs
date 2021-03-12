// <copyright file="AspireUseConfigure.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Aspire.Logger;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Aspire 配置项.
    /// </summary>
    public class AspireUseConfigure
    {
        /// <summary>
        /// Gets or sets Swagger UI 名称.
        /// </summary>
        [NotNull]
        public string SwaggerUiName { get; set; }

        /// <summary>
        /// Gets or sets 终节点路由配置.
        /// </summary>
        [NotNull]
        public Action<IEndpointRouteBuilder> EndpointRouteConfigure { get; set; }

        /// <summary>
        /// Gets or sets Logger Configure.
        /// </summary>
        [NotNull]
        public ILoggerConfigure LoggerConfigure { get; set; }
    }
}