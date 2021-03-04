using System;
using System.Diagnostics;
using System.Linq;

using Newtonsoft.Json;

namespace Aspire
{
    internal class GlobalResponse
    {
        public int Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Message { get; set; }

        public object Result { get; set; } = new { };

#if DEBUG
        [JsonIgnore]
        public object StackTrace { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] StackTraceText {
            get {
                if (StackTrace is null) return null;

                return StackTrace switch {
                    FriendlyException friendlyException => friendlyException.StackTrace.ToString()
                        .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => !x.Contains(
                            $"{nameof(FriendlyThrowException)}.{nameof(FriendlyThrowException.ThrowException)}"))
                        .Where(x => !x.Contains("System.Runtime"))
                        .Where(x => !x.Contains("Microsoft.AspNetCore"))
                        .Where(x => !x.Contains("Swashbuckle.AspNetCore"))
                        .Where(x => !x.Contains("System.Threading"))
                        .ToArray(),
                    Exception exception => exception.StackTrace
                        ?.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray(),
                    EnhancedStackTrace _ => StackTrace.ToString()
                        ?.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray(),
                    StackTrace _ => StackTrace.ToString()
                        ?.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray(),
                    _ => new[] { StackTrace.ToString() }
                };
            }
        }
#endif
    }
}
