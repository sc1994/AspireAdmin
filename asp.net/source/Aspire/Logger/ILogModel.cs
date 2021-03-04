using System;

namespace Aspire.Logger
{
    /// <inheritdoc />
    public interface ILogModel : ILogModel<Guid>
    {

    }

    /// <summary>
    /// 日志 模型
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface ILogModel<TId>
    {
        /// <summary>
        /// id
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// traceId
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 类 名称
        /// </summary>
        public string ClassName { get; set; }

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
