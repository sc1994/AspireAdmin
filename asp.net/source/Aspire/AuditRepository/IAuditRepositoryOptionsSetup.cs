using Microsoft.Extensions.DependencyInjection;

namespace Aspire
{
    /// <summary>
    /// 审计仓储启动选项
    /// </summary>
    public interface IAuditRepositoryOptionsSetup
    {
        /// <summary>
        /// 添加审计仓储
        /// </summary>
        /// <param name="services"></param>
         void AddAuditRepository(IServiceCollection services);
    }
}
