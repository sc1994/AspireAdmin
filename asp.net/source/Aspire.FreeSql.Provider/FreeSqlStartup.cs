using FreeSql;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire.FreeSql.Provider
{
    public static class FreeSqlStartup
    {
        public static IServiceCollection AddFreeSql(
            this IServiceCollection services,
            string connectionString,
            DataType dataType)
        {
            services.AddSingleton<IFreeSql>(serviceProvider => {
                var freeSql = new FreeSqlBuilder()
                .UseConnectionString(dataType, connectionString)
#if DEBUG
                .UseAutoSyncStructure(true) // 本地自动同步数据库
#endif
                .Build();
                return freeSql;
            });

            return services;
        }
    }
}