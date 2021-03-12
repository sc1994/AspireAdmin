// <copyright file="LogWriterHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Net;
    using Aspire.Authenticate;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// 日志写入帮助.
    /// </summary>
    public class LogWriterHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUser currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriterHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        /// <param name="currentUser">Current User.</param>
        public LogWriterHelper(IHttpContextAccessor httpContextAccessor, ICurrentUser currentUser)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 获取部分标准参数.
        /// </summary>
        /// <returns>
        /// <para>Api Method.</para>
        /// <para>Api Router.</para>
        /// <para>Trace Id.</para>
        /// <para>Client Address.</para>
        /// <para>Server Address.</para>
        /// </returns>
        public (string apiMethod, string apiRouter, string traceId, string clientAddress, string serverAddress, string userAccount) GetPartialStandardParams()
        {
            var cxt = this.httpContextAccessor.HttpContext;
            if (cxt is null)
            {
                goto Default;
            }

            return (cxt.Request.Method,
                cxt.Request.Path.Value,
                GetTrace(cxt),
                $"{RemoveIpV6Zero(cxt.Connection.RemoteIpAddress)}:{cxt.Connection.RemotePort}",
                $"{RemoveIpV6Zero(cxt.Connection.LocalIpAddress)}:{cxt.Connection.LocalPort}",
                this.currentUser.Account);

        Default:
            return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private static string GetTrace(HttpContext cxt)
        {
            if (cxt?.Items is null)
            {
                return null;
            }

            const string traceKey = "Aspire.Logger.TraceId";

            if (cxt.Items.TryGetValue(traceKey, out var traceId)
                && traceId is string id
                && !string.IsNullOrWhiteSpace(id))
            {
                return id;
            }

            lock (traceKey)
            {
                if (cxt.Items.TryGetValue(traceKey, out var traceId2)
                    && traceId2 is string id2
                    && !string.IsNullOrWhiteSpace(id2))
                {
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
            if (ip.StartsWith("::ffff:"))
            {
                return ip.Remove(0, 7);
            }

            return ip;
        }
    }
}