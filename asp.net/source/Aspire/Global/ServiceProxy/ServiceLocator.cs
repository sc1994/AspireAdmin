namespace Aspire
{
    using System;

    /// <summary>
    /// 服务定位
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProviderProxy _diProxy;

        /// <summary>
        /// Gets DI 服务提供 代理 实例.
        /// </summary>
        public static IServiceProviderProxy ServiceProvider => _diProxy ?? throw new Exception("请先调用Initialize初始化服务");

        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="proxy">Proxy.</param>
        internal static void Initialize(IServiceProviderProxy proxy)
        {
            _diProxy = proxy;
        }
    }
}