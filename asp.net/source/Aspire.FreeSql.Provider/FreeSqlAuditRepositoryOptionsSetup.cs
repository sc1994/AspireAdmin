using FreeSql;

using Microsoft.Extensions.DependencyInjection;

namespace Aspire.FreeSql.Provider
{
    public class FreeSqlAuditRepositoryOptionsSetup : IAuditRepositoryOptionsSetup
    {
        private readonly string _connectionString;
        private readonly DataType _dataType;

        public FreeSqlAuditRepositoryOptionsSetup(string connectionString, DataType dataType)
        {
            _connectionString = connectionString;
            _dataType = dataType;
        }

        public void AddAuditRepository(IServiceCollection services)
        {
            services.AddSingleton(serviceProvider => {
                var freeSql = new FreeSqlBuilder()
                .UseConnectionString(_dataType, _connectionString)
#if DEBUG
                .UseAutoSyncStructure(true) // 本地自动同步数据库
#endif
                .Build();
                return freeSql;
            });

            services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));
            services.AddScoped(typeof(IAuditRepository<,>), typeof(AuditRepository<,>));
        }
    }
}