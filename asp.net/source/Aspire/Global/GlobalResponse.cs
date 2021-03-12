// <copyright file="GlobalResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Global Response.
    /// </summary>
    internal class GlobalResponse
    {
        /// <summary>
        /// Gets or sets Code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets Messages.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Messages { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Result.
        /// </summary>
        public object Result { get; set; } = new { };

#if DEBUG
        /// <summary>
        /// Gets or sets Stack Trace.
        /// </summary>
        [JsonIgnore]
        public object StackTrace { get; set; }

        /// <summary>
        /// Gets Stack Trace Text.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] StackTraceText
        {
            get
            {
                if (this.StackTrace is null)
                {
                    return null;
                }

                return this.StackTrace switch
                {
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
                    EnhancedStackTrace _ => this.StackTrace.ToString()
                        ?.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray(),
                    StackTrace _ => this.StackTrace.ToString()
                        ?.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .ToArray(),
                    _ => new[] { this.StackTrace.ToString() }
                };
            }
        }
#endif
    }
}
