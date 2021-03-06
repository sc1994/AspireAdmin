using System;
using Aspire.SystemLog;

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    /// <summary>
    /// 日志 查询过滤器
    /// </summary>
    public class SystemLogFilterInputDto : SystemLogCommonDto, ISystemLogFilterInputDto
    {
        public DateTime[] CreatedAtRange { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}