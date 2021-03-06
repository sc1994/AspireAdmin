// <copyright file="ISystemLogFilterInputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.SystemLog
{
    using System;

    /// <summary>
    /// 日志 查询过滤器.
    /// </summary>
    public interface ISystemLogFilterInputDto : IPageInputDto, ISystemLogCommonDto
    {
        /// <summary>
        /// Gets or sets 创建时间 范围.
        /// </summary>
        DateTime[] CreatedAtRange { get; set; }
    }
}