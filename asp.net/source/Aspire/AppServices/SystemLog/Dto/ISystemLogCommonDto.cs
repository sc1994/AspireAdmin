// <copyright file="ISystemLogCommonDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.SystemLog
{
    /// <summary>
    /// 系统日志 通用实体.
    /// </summary>
    public interface ISystemLogCommonDto
    {
        /// <summary>
        /// Gets or sets api method.
        /// </summary>
        string ApiMethod { get; set; }

        /// <summary>
        /// Gets or sets api router.
        /// </summary>
        string ApiRouter { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets traceId.
        /// </summary>
        string TraceId { get; set; }

        /// <summary>
        /// Gets or sets 过滤1.
        /// </summary>
        string Filter1 { get; set; }

        /// <summary>
        /// Gets or sets 过滤2.
        /// </summary>
        string Filter2 { get; set; }

        /// <summary>
        /// Gets or sets 远端 地址.
        /// </summary>
        string ClientAddress { get; set; }

        /// <summary>
        /// Gets or sets 服务端 地址.
        /// </summary>
        string ServerAddress { get; set; }

        /// <summary>
        /// Gets or sets 等级.
        /// </summary>
        LogLevelEnum? Level { get; set; }
    }
}
