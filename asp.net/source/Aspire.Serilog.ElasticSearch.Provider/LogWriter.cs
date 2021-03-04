using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

using Aspire.Logger;

using Microsoft.AspNetCore.Http;

using Serilog;

namespace Aspire.Serilog.ElasticSearch.Provider
{
    static internal class LogInfoStore
    {
        public static ConcurrentDictionary<Type, string> TypeInfos = new ConcurrentDictionary<Type, string>();


        public const string LogTemplate = "{traceId} {appName}\r\n" +
                                          "   id: {id}\r\n" +
                                          "   f1: {f1}\r\n" +
                                          "   f2: {f2}\r\n" +
                                          "   {body}";

        public const string TraceKey = "Aspire.Serilog.ElasticSearch.Provider.TraceKey";
    }

    public class LogWriter<TCurrentClass> : ILogWriter<TCurrentClass>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public LogWriter(IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// 获取追踪guid, 此值藏于 HttpContext.Items 中 . 
        /// </summary>
        private string GetTrace()
        {
            var cxt = _httpContextAccessor.HttpContext;
            if (cxt?.Items == null) {
                return null;
            }

            var sign = 0;

            Again:
            if (cxt.Items.TryGetValue(LogInfoStore.TraceKey, out var trace)
                && trace is string t
                && !string.IsNullOrWhiteSpace(t)) {
                return t;
            }

            lock (LogInfoStore.TraceKey) {
                if (sign++ == 0) {
                    goto Again;
                }

                t = Guid.NewGuid().ToString();
                cxt.Items.Add(LogInfoStore.TraceKey, t);
                return t;
            }
        }

        private object[] GetLogParams(string body, string filter1 = null, string filter2 = null)
        {
            var typeInfo = LogInfoStore.TypeInfos.GetOrAdd(typeof(TCurrentClass), type =>
                type.GetCustomAttribute<DescriptionAttribute>()?.Description
                ?? $"{type.Namespace ?? "Unknown"}.{type.Name}");

            return new object[] {
                GetTrace(),
                typeInfo,
                Guid.NewGuid().ToString(),
                filter1,
                filter2,
                body
            };
        }

        public async Task InfoAsync(string body, string filter1 = null, string filter2 = null)
        {
            _logger.Information(LogInfoStore.LogTemplate, GetLogParams(body, filter1, filter2));
        }

        public async Task WarnAsync(string body, string filter1 = null, string filter2 = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task ErrorAsync(Exception ex, string body, string filter1 = null, string filter2 = null)
        {
            _logger.Error(ex, LogInfoStore.LogTemplate, GetLogParams(body, filter1, filter2));
        }
    }
}
