using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire.Logger
{
    /// <summary>
    /// logger 启动配置
    /// </summary>
    public interface ILoggerOptionsSetup
    {
        /// <summary>
        /// add logger 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        void AddLogger(IServiceCollection services, IConfiguration configuration);
    }
}
