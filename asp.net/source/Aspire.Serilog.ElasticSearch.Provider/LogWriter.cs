using System;
using System.Threading.Tasks;

using Serilog;

namespace Aspire.Serilog.ElasticSearch.Provider
{
    public class LogWriter : ILogWriter
    {
        private readonly LogWriterHelper _writerHelper;
        private readonly ILogger _logger;
        private const string LogTemplate = "[{clientAddress}->{serverAddress}] {traceId} {apiMethod} {apiRouter}\r\n" +
                                           "   f1: {f1}\r\n" +
                                           "   f2: {f2}\r\n" +
                                           "   {body}";

        public LogWriter(ILogger logger, LogWriterHelper writerHelper)
        {
            _writerHelper = writerHelper;
            _logger = logger;
        }

        private object[] GetLogParams(string body, string filter1 = null, string filter2 = null)
        {
            var (apiMethod, apiRouter, traceId, clientAddress, serverAddress) = _writerHelper.GetPartialStandardParams();

            return new object[] {
                clientAddress,
                serverAddress,
                traceId,
                apiMethod,
                apiRouter,
                filter1,
                filter2,
                body
            };
        }

        public async Task InfoAsync(string body, string filter1 = null, string filter2 = null)
        {
            await Task.Run(() => {
                _logger.Information(LogTemplate, GetLogParams(body, filter1, filter2));
            });
        }

        public async Task WarnAsync(string body, string filter1 = null, string filter2 = null)
        {
            await Task.Run(() => {
                _logger.Warning(LogTemplate, GetLogParams(body, filter1, filter2));
            });
        }

        public async Task ErrorAsync(Exception ex, string body, string filter1 = null, string filter2 = null)
        {
            await Task.Run(() => {
                _logger.Error(ex, LogTemplate, GetLogParams(body, filter1, filter2));
            });
        }
    }
}
