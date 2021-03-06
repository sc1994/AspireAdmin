using System;

using Aspire.Logger;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Aspire.Serilog.ElasticSearch.Provider
{
    public class SerilogElasticSearchOptionsSetup : ILoggerOptionsSetup
    {
        public void AddLogger(IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                  //.ReadFrom.Configuration(configuration)
                  .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration.GetConnectionString("ElasticSearch"))) {
                      CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: false, renderMessageTemplate: false),
                      IndexFormat = configuration.GetConnectionString("ElasticSearchIndex") + "{0:yyyyMMdd}"
                  })
#if DEBUG
                  .WriteTo.Console(LogEventLevel.Information)
#endif
                  .CreateLogger();

            services.AddSingleton(logger);
            services.AddSingleton<ILogger>(logger);
            services.AddScoped(typeof(ILogWriter), typeof(LogWriter));
        }
    }
}
