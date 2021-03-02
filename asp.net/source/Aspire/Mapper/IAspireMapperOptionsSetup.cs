using Microsoft.Extensions.DependencyInjection;

namespace Aspire.Mapper
{
    /// <summary>
    /// Aspire Mapper 启动
    /// </summary>
    public interface IAspireMapperOptionsSetup
    {
        /// <summary>
        /// 添加 Aspire Mapper
        /// <para>必须声明如何实现IAspireMapperStartup</para>
        /// </summary>
        /// <param name="services"></param>
        void AddAspireMapper(IServiceCollection services);
    }
}
