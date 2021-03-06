using System;

using Aspire.SystemLog;

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    public class SystemLogFilterOutputDto : SystemLogCommonDto, ISystemLogFilterOutputDto<string>
    {
        public DateTime CreatedAt { get; set; }
        public double TickForRequest { get; set; }
        public string Body { get; set; }
        public string Id { get; set; }
    }
}
