using Microsoft.Extensions.DependencyInjection;

namespace Aspire.FreeSql.Provider
{
    public static class FreeSqlStartup
    {
        public static IServiceCollection AddFreeSql(this IServiceCollection services)
        {
            return services;
        }
    }
}