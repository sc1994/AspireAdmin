
using Aspire.SystemLog;

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    public class SystemLogCommonDto : ISystemLogCommonDto
    {
        public string ApiMethod { get; set; }

        public string ApiRouter { get; set; }

        public string TraceId { get; set; }

        public string Filter1 { get; set; }

        public string Filter2 { get; set; }

        public string ClientAddress { get; set; }

        public string ServerAddress { get; set; }
    }
}
