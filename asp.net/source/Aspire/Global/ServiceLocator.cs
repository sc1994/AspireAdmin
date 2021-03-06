// <copyright file="ServiceLocator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// di 服务提供 代理.
    /// </summary>
    public interface IServiceProviderProxy
    {
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>();
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetServices<T>();
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        object GetService(Type type);
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetServices(Type type);
    }

    /// <summary>
    /// 服务定位
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProviderProxy _diProxy;

        /// <summary>
        /// di 服务提供 代理 实例
        /// </summary>
        public static IServiceProviderProxy ServiceProvider => _diProxy ?? throw new Exception("请先调用Initialize初始化服务");

        internal static void Initialize(IServiceProviderProxy proxy)
        {
            _diProxy = proxy;
        }
    }

    internal class HttpContextServiceProviderProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextServiceProviderProxy(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public T GetService<T>()
        {
            return GetHttpContext().RequestServices.GetService<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            return GetHttpContext().RequestServices.GetServices<T>();
        }

        public object GetService(Type type)
        {
            return GetHttpContext().RequestServices.GetService(type);
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return GetHttpContext().RequestServices.GetServices(type);
        }

        private HttpContext GetHttpContext()
        {
            return _contextAccessor.HttpContext ?? throw new NotSupportedException("只能在http请求中使用此方式获取服务");
        }
    }
}
