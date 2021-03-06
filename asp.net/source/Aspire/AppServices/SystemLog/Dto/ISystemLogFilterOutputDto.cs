namespace Aspire.SystemLog
{
    using System;

    /// <summary>
    /// 系统日志 过滤 输出
    /// </summary>
    public interface ISystemLogFilterOutputDto<TId> : ISystemLogCommonDto
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// 对于请求开始消逝的毫秒数
        /// </summary>
        double TickForRequest { get; set; }

        /// <summary>
        /// 主体
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        TId Id { get; set; }
    }
}
