// <copyright file="LogWriter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider
{
    using System;
    using System.Linq;
    using Aspire.Cache;
    using global::Serilog;

    /// <summary>
    /// Log Writer.
    /// </summary>
    public class LogWriter : ILogWriter
    {
        /// <summary>
        /// Redis Key Api Method.
        /// </summary>
        internal const string RedisKeyApiMethod = "Aspire:Serilog:ElasticSearch:Select:ApiMethod";

        /// <summary>
        /// Redis Key Api Router.
        /// </summary>
        internal const string RedisKeyApiRouter = "Aspire:Serilog:ElasticSearch:Select:ApiRouter";

        /// <summary>
        /// Redis Key Server Address.
        /// </summary>
        internal const string RedisKeyServerAddress = "Aspire:Serilog:ElasticSearch:Select:ServerAddress";

        private const string LogTemplate = "[{clientAddress}->{serverAddress}] [{f1}] [{f2}]" +
                                           "\r\n\t{apiMethod}: {apiRouter}" +
                                           "\r\n\ttrace  : {traceId}" +
                                           "\r\n\tmessage: {message}";

        private readonly LogWriterHelper writerHelper;
        private readonly IAspireRedis redis;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class.
        /// </summary>
        /// <param name="logger">Serilog.ILogger.</param>
        /// <param name="writerHelper">Log Writer Helper.</param>
        /// <param name="redis">Redis.</param>
        public LogWriter(
            ILogger logger,
            LogWriterHelper writerHelper,
            IAspireRedis redis)
        {
            this.writerHelper = writerHelper;
            this.redis = redis;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void Information(string message, string filter1 = null, string filter2 = null)
        {
            this.logger.Information(LogTemplate, this.GetLogParams(message, filter1, filter2));
        }

        /// <inheritdoc/>
        public void Warning(string message, string filter1 = null, string filter2 = null)
        {
            this.logger.Warning(LogTemplate, this.GetLogParams(message, filter1, filter2));
        }

        /// <inheritdoc/>
        public void Error(Exception ex, string message, string filter1 = null, string filter2 = null)
        {
            var @params = this.GetLogParams(message, filter1, filter2)
                    .Append(ex.ToString())
                    .ToArray();
            this.logger.Error(LogTemplate + "\r\n····error  :{error}", @params);
        }

        private object[] GetLogParams(string message, string filter1 = null, string filter2 = null)
        {
            var (apiMethod, apiRouter, traceId, clientAddress, serverAddress) = this.writerHelper.GetPartialStandardParams();

            // 收集内容 供可选择项目
            this.redis.AddSetMembers(RedisKeyApiMethod, apiMethod);
            this.redis.AddSetMembers(RedisKeyApiRouter, apiRouter);
            this.redis.AddSetMembers(RedisKeyServerAddress, serverAddress);

            return new object[]
            {
                clientAddress,
                serverAddress,
                filter1,
                filter2,
                apiMethod,
                apiRouter,
                traceId,
                message,
            };
        }
    }
}
