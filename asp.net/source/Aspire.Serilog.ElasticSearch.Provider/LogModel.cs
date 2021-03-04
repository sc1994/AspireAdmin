using System;

using Aspire.Logger;

namespace Aspire.Serilog.ElasticSearch.Provider
{
    public class LogModel : ILogModel<string>
    {
        public string Id { get; set; }

        public LogLevelEnum Level { get; set; }

        public string Body { get; set; }

        public string TraceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ClassName { get; set; }

        public string Filter1 { get; set; }

        public string Filter2 { get; set; }
    }
}
