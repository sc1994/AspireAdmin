// <copyright file="SystemLogCommonDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    using Aspire.SystemLog;

    /// <summary>
    /// Common Dto.
    /// </summary>
    public class SystemLogCommonDto : ISystemLogCommonDto
    {
        /// <inheritdoc/>
        public string ApiMethod { get; set; }

        /// <inheritdoc/>
        public string ApiRouter { get; set; }

        /// <inheritdoc/>
        public string Title { get; set; }

        /// <inheritdoc/>
        public string TraceId { get; set; }

        /// <inheritdoc/>
        public string Filter1 { get; set; }

        /// <inheritdoc/>
        public string Filter2 { get; set; }

        /// <inheritdoc/>
        public string ClientAddress { get; set; }

        /// <inheritdoc/>
        public string ServerAddress { get; set; }

        /// <inheritdoc/>
        public LogLevelEnum? Level { get; set; }
    }
}
