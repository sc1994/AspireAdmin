using System;

namespace Aspire.Logger
{
    /// <summary>
    /// 日志 查询过滤器
    /// </summary>
    public class LogQueryFilter
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// traceId
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 创建时间 范围
        /// </summary>
        public DateTime[] CreatedAtRange { get; set; }

        /// <summary>
        /// 过滤1
        /// </summary>
        public string Filter1 { get; set; }

        /// <summary>
        /// 过滤2
        /// </summary>
        public string Filter2 { get; set; }
    }
}