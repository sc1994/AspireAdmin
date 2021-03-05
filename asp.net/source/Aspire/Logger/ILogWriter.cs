using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Aspire
{
    /// <summary>
    /// 日志写入帮助
    /// </summary>
    public class LogWriterHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 日志写入帮助
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public LogWriterHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static string GetTrace(HttpContext cxt)
        {
            if (cxt?.Items == null) {
                return null;
            }

            var sign = 0;

            const string traceKey = "Aspire.Logger.TraceId";

            Again:
            if (cxt.Items.TryGetValue(traceKey, out var trace)
                && trace is string t
                && !string.IsNullOrWhiteSpace(t)) {
                return t;
            }

            lock (traceKey) {
                if (sign++ == 0) {
                    goto Again;
                }

                t = Guid.NewGuid().ToString();
                cxt.Items.Add(traceKey, t);
                return t;
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
        /// <returns></returns>
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

    /// <summary>
    /// 日志 写入
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// write info
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        Task InfoAsync(string body, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write info
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public Task InfoAsync(object body, string filter1 = null, string filter2 = null)
        {
            return InfoAsync(body.SerializeObject(), filter1, filter2);
        }

        /// <summary>
        /// write warn
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        Task WarnAsync(string body, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write warn
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public Task WarnAsync(object body, string filter1 = null, string filter2 = null)
        {
            return WarnAsync(body.SerializeObject(), filter1, filter2);
        }

        /// <summary>
        /// write error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        Task ErrorAsync(Exception ex, string body, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public Task ErrorAsync(Exception ex, object body, string filter1 = null, string filter2 = null)
        {
            return ErrorAsync(ex, body.SerializeObject(), filter1, filter2);
        }
    }
}
