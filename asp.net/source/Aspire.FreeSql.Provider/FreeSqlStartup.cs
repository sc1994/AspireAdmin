using System;

using FreeSql;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeSqlStartup
    {
        static internal IServiceProvider ServiceProvider;

        public static IServiceCollection AddFreeSql(
            this IServiceCollection services,
            string connectionString,
            DataType dataType)
        {
            services.AddSingleton(serviceProvider => {
                ServiceProvider = serviceProvider;
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