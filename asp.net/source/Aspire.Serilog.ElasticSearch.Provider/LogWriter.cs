// <copyright file="LogWriter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider
{
    using System;
    using System.Linq;
    using global::Serilog;

    /// <summary>
    /// Log Writer.
    /// </summary>
    public class LogWriter : ILogWriter
    {
        private const string LogTemplate = "{apiMethod}  {apiRouter}" +
                                           "\r\n[{clientAddress}->{serverAddress}] [{f1}] [{f2}]" +
                                           "\r\ntitle  : {title}" +
                                           "\r\naccount: {account}" +
                                           "\r\ntrace  : {traceId}" +
                                           "\r\nmessage: {message}" +
                                           "\r\n";

        private readonly LogWriterHelper writerHelper;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class.
        /// </summary>
        /// <param name="logger">Serilog.ILogger.</param>
        /// <param name="writerHelper">Log Writer Helper.</param>
        public LogWriter(
            ILogger logger,
            LogWriterHelper writerHelper)
        {
            this.writerHelper = writerHelper;
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
            LogItemsStore.AddItems(new { apiMethod, apiRouter, serverAddress, title });

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
