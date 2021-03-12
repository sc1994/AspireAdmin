// <copyright file="SerilogElasticSearchOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider
{
    using System;
    using Aspire.Logger;
    using global::Serilog;
    using global::Serilog.Events;
    using global::Serilog.Formatting.Elasticsearch;
    using global::Serilog.Sinks.Elasticsearch;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Serilog ElasticSearch Options Setup.
    /// </summary>
    public class SerilogElasticSearchOptionsSetup : ILoggerOptionsSetup
    {
        /// <inheritdoc/>
        public void AddLogger(IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                  .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration.GetConnectionString("ElasticSearch")))
                  {
                      CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: false, renderMessageTemplate: false),
                      IndexFormat = configuration.GetConnectionString("ElasticSearchIndex") + "{0:yyyyMMdd}",
                  })
#if DEBUG
                  .WriteTo.Console(LogEventLevel.Information)
#endif
                  .CreateLogger();

            services.AddSingleton(logger);
            services.AddSingleton<ILogger>(logger);
            services.AddScoped(typeof(ILogWriter), typeof(LogWriter));
            services.AddSingleton<LogItemsStore, LogItemsStore>();
        }
    }
}
