// <copyright file="SystemLogFilterInputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    using System;
    using Aspire.SystemLog;

    /// <summary>
    /// 日志 查询过滤器.
    /// </summary>
    public class SystemLogFilterInputDto : SystemLogCommonDto, ISystemLogFilterInputDto
    {
        /// <inheritdoc/>
        public DateTime[] CreatedAtRange { get; set; }

        /// <inheritdoc/>
        public int PageIndex { get; set; }

        /// <inheritdoc/>
        public int PageSize { get; set; }
    }
}