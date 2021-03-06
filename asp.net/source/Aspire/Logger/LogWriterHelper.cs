using System;
using System.Net;

using Microsoft.AspNetCore.Http;

namespace Aspire
{
    /// <summary>
    /// 日志写入帮助.
    /// </summary>
    public class LogWriterHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        public LogWriterHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static string GetTrace(HttpContext cxt)
        {
            if (cxt?.Items == null) {
                return null;
            }

            const string traceKey = "Aspire.Logger.TraceId";

            if (cxt.Items.TryGetValue(traceKey, out var traceId)
                && traceId is string id
                && !string.IsNullOrWhiteSpace(id)) {
                return id;
            }

            lock (traceKey) {
                if (cxt.Items.TryGetValue(traceKey, out var traceId2)
                    && traceId2 is string id2
                    && !string.IsNullOrWhiteSpace(id2)) {
                    return id2;
                }

                var newId = Guid.NewGuid().ToString();
                cxt.Items.Add(traceKey, newId);
                return newId;
            }
        }

        private static string RemoveIpV6Zero(IPAddress ipAddress)
        {
            var ip = ipAddress?.ToString() ?? "Unknown";
            if (ip.StartsWith("::ffff:")) {
                return ip.Remove(0, 7);
            }
            return ip;
        }

        /// <summary>
        /// 获取部分标准参数
        /// </summary>
        public (string apiMethod, string apiRouter, string traceId, string clientAddress, string serverAddress) GetPartialStandardParams()
        {
            var cxt = _httpContextAccessor.HttpContext;
            if (cxt is null) goto Default;

            return (cxt.Request.Method,
                cxt.Request.Path.Value,
                GetTrace(cxt),
                $"{RemoveIpV6Zero(cxt.Connection.RemoteIpAddress)}:{cxt.Connection.RemotePort}",
                $"{RemoveIpV6Zero(cxt.Connection.LocalIpAddress)}:{cxt.Connection.LocalPort}");


            Default:
            return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
    }
}