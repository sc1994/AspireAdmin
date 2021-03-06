// <copyright file="ILogWriter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;

    /// <summary>
    /// 日志 写入.
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// write info.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="filter1">filter1.</param>
        /// <param name="filter2">filter2.</param>
        void Information(string message, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write info.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="filter1">filter1.</param>
        /// <param name="filter2">filter2.</param>
        public void Information(object message, string filter1 = null, string filter2 = null)
        {
            this.Information(message.SerializeObject(), filter1, filter2);
        }

        /// <summary>
        /// write warn.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="filter1">filter1.</param>
        /// <param name="filter2">filter2.</param>
        void Warning(string message, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write warn.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="filter1">filter1.</param>
        /// <param name="filter2">filter2.</param>
        public void Warning(object message, string filter1 = null, string filter2 = null)
        {
            this.Warning(message.SerializeObject(), filter1, filter2);
        }

        /// <summary>
        /// write error.
        /// </summary>
        /// <param name="ex">exception.</param>
        /// <param name="message">message.</param>
        /// <param name="filter1">filter1.</param>
        /// <param name="filter2">filter2.</param>
        void Error(Exception ex, string message, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write error.
        /// </summary>
        /// <param name="ex">exception.</param>
        /// <param name="message">message.</param>
        /// <param name="filter1">filter1.</param>
        /// <param name="filter2">filter2.</param>
        public void Error(Exception ex, object message, string filter1 = null, string filter2 = null)
        {
            this.Error(ex, message.SerializeObject(), filter1, filter2);
        }
    }
}
