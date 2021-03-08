// <copyright file="SystemLogFilterOutputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    using System;
    using Aspire.SystemLog;

    /// <summary>
    /// Filter Output.
    /// </summary>
    public class SystemLogFilterOutputDto : SystemLogCommonDto, ISystemLogFilterOutputDto<string>
    {
        /// <inheritdoc/>
        public DateTime CreatedAt { get; set; }

        /// <inheritdoc/>
        public double TickForRequest { get; set; }

        /// <inheritdoc/>
        public string Message { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }
    }
}
