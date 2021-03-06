namespace Aspire.SystemLog
{
    /// <summary>
    /// 系统日志 通用实体
    /// </summary>
    public interface ISystemLogCommonDto
    {
        /// <summary>
        /// api method
        /// </summary>
        string ApiMethod { get; set; }

        /// <summary>
        /// api router
        /// </summary>
        string ApiRouter { get; set; }

        /// <summary>
        /// traceId
        /// </summary>
        string TraceId { get; set; }

        /// <summary>
        /// 过滤1
        /// </summary>
        string Filter1 { get; set; }

        /// <summary>
        /// 过滤2
        /// </summary>
        string Filter2 { get; set; }

        /// <summary>
        /// 远端 地址
        /// </summary>
        string ClientAddress { get; set; }

        /// <summary>
        /// 服务端 地址
        /// </summary>
        string ServerAddress { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        LogLevelEnum? Level { get; set; }
    }
}
