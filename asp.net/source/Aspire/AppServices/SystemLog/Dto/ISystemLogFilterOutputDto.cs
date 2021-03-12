// <copyright file="ISystemLogFilterOutputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.SystemLog
{
    using System;

    /// <summary>
    /// 系统日志 过滤 输出.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Primary Key.</typeparam>
    public interface ISystemLogFilterOutputDto<TPrimaryKey> : ISystemLogCommonDto
    {
        /// <summary>
        /// Gets or sets 创建时间.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets 对于请求开始消逝的毫秒数.
        /// </summary>
        double TickForRequest { get; set; }

        /// <summary>
        /// Gets or sets Messages.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
