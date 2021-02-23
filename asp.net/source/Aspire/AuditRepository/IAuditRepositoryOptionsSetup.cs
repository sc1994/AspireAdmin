using Microsoft.Extensions.DependencyInjection;

namespace Aspire
{
    public interface IAuditRepositoryOptionsSetup
    {
        public void AddAuditRepository(IServiceCollection services);
    }
}
