using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire
{
    public interface IServiceProviderProxy
    {
        T GetService<T>();
        IEnumerable<T> GetServices<T>();
        object GetService(Type type);
        IEnumerable<object> GetServices(Type type);
    }

    public static class ServiceLocator
    {
        private static IServiceProviderProxy _diProxy;

        public static IServiceProviderProxy ServiceProvider => _diProxy ?? throw new Exception("请先调用Initialize初始化服务");

        public static void Initialize(IServiceProviderProxy proxy)
        {
            _diProxy = proxy;
        }
    }

    public class HttpContextServiceProviderProxy : IServiceProviderProxy
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
