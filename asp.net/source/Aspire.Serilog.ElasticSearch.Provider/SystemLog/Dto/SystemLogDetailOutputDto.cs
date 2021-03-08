// <copyright file="SystemLogDetailOutputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    using Aspire.SystemLog;

    /// <summary>
    /// 系统日志 详情 输出.
    /// </summary>
    public class SystemLogDetailOutputDto : SystemLogFilterOutputDto, ISystemLogDetailOutputDto<string>
    {
    }
}
