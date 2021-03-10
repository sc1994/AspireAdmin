// <copyright file="HttpContextServiceProviderProxy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Http Context Service Provider Proxy.
    /// </summary>
    internal class HttpContextServiceProviderProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpContextServiceProviderProxy"/> class.
        /// </summary>
        /// <param name="contextAccessor">Http Context Accessor.</param>
        public HttpContextServiceProviderProxy(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <inheritdoc/>
        public T GetService<T>()
        {
            return this.GetHttpContext().RequestServices.GetService<T>();
        }

        /// <inheritdoc/>
        public IEnumerable<T> GetServices<T>()
        {
            return this.GetHttpContext().RequestServices.GetServices<T>();
        }

        /// <inheritdoc/>
        public object GetService(Type type)
        {
            return this.GetHttpContext().RequestServices.GetService(type);
        }

        /// <inheritdoc/>
        public IEnumerable<object> GetServices(Type type)
        {
            return this.GetHttpContext().RequestServices.GetServices(type);
        }

        private HttpContext GetHttpContext()
        {
            return this.contextAccessor.HttpContext
                ?? throw new NotSupportedException("只能在http请求中使用此方式获取服务");
        }
    }
}
