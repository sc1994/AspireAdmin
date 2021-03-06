using System;
using System.Linq;
using Serilog;

namespace Aspire.Serilog.ElasticSearch.Provider
{
    public class LogWriter : ILogWriter
    {
        private readonly LogWriterHelper _writerHelper;
        private readonly ILogger _logger;
        private const string LogTemplate = "[{clientAddress}->{serverAddress}] [{f1}] [{f2}]" +
                                           "\r\n\t{apiMethod}: {apiRouter}" +
                                           "\r\n\ttrace  : {traceId}" +
                                           "\r\n\tmessage: {message}";

        public LogWriter(ILogger logger, LogWriterHelper writerHelper)
        {
            this._writerHelper = writerHelper;
            this._logger = logger;
        }

        private object[] GetLogParams(string message, string filter1 = null, string filter2 = null)
        {
            (var apiMethod, var apiRouter, var traceId, var clientAddress, var serverAddress) = this._writerHelper.GetPartialStandardParams();

            return new object[] {
                clientAddress,
                serverAddress,
                filter1,
                filter2,
                apiMethod,
                apiRouter,
                traceId,
                message
            };
        }

        public void Information(string message, string filter1 = null, string filter2 = null)
        {
            this._logger.Information(LogTemplate, this.GetLogParams(message, filter1, filter2));
        }

        public void Warning(string message, string filter1 = null, string filter2 = null)
        {
            this._logger.Warning(LogTemplate, this.GetLogParams(message, filter1, filter2));
        }

        public void Error(Exception ex, string message, string filter1 = null, string filter2 = null)
        {
            var @params = this.GetLogParams(message, filter1, filter2)
                    .Append(ex.ToString())
                    .ToArray();
            this._logger.Error(LogTemplate + "\r\n····error  :{error}", @params);
        }
    }
}
