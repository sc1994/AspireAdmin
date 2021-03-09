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

        /// <summary>
        /// Redis Key Title.
        /// </summary>
        internal const string RedisKeyTitle = "Aspire:Serilog:ElasticSearch:Select:Title";

        private const string LogTemplate = "{apiMethod}  {apiRouter}" +
                                           "\r\n\t[{clientAddress}->{serverAddress}] [{f1}] [{f2}]" +
                                           "\r\n\ttitle  : {title}" +
                                           "\r\n\taccount: {account}" +
                                           "\r\n\ttrace  : {traceId}" +
                                           "\r\n\tmessage: {message}";

        private readonly LogWriterHelper writerHelper;
        private readonly IAspireCache cache;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class.
        /// </summary>
        /// <param name="logger">Serilog.ILogger.</param>
        /// <param name="writerHelper">Log Writer Helper.</param>
        /// <param name="cache">Redis.</param>
        public LogWriter(
            ILogger logger,
            LogWriterHelper writerHelper,
            IAspireCache cache)
        {
            this.writerHelper = writerHelper;
            this.cache = cache;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void Information(string title, string message, string filter1 = null, string filter2 = null)
        {
            this.logger.Information(LogTemplate, this.GetLogParams(title, message, filter1, filter2));
        }

        /// <inheritdoc/>
        public void Warning(string title, string message, string filter1 = null, string filter2 = null)
        {
            this.logger.Warning(LogTemplate, this.GetLogParams(title, message, filter1, filter2));
        }

        /// <inheritdoc/>
        public void Error(Exception ex, string message, string filter1 = null, string filter2 = null)
        {
            var @params = this.GetLogParams(ex.Message, message, filter1, filter2)
                    .Append(ex.ToString())
                    .ToArray();
            this.logger.Error(LogTemplate + "\r\n\terror  :{error}", @params);
        }

        private object[] GetLogParams(string title, string message, string filter1 = null, string filter2 = null)
        {
            var (apiMethod, apiRouter, traceId, clientAddress, serverAddress, userAccount) = this.writerHelper.GetPartialStandardParams();

            // 收集内容 供可选择项目
            this.cache.AddSetMembers(RedisKeyApiMethod, apiMethod);
            this.cache.AddSetMembers(RedisKeyApiRouter, apiRouter);
            this.cache.AddSetMembers(RedisKeyServerAddress, serverAddress);
            this.cache.AddSetMembers(RedisKeyTitle, title);

            return new object[]
            {
                apiMethod,
                apiRouter,
                clientAddress,
                serverAddress,
                filter1 ?? string.Empty,
                filter2 ?? string.Empty,
                title,
                userAccount,
                traceId,
                message,
            };
        }
    }
}
