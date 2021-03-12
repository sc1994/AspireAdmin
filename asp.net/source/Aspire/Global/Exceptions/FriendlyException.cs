// <copyright file="FriendlyException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 友好异常.
    /// </summary>
    public class FriendlyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyException"/> class.
        /// 实例化 异常.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="stackTrace">Stack Trace.</param>
        /// <param name="title">Title.</param>
        /// <param name="messages">Messages.</param>
        public FriendlyException(
            int code,
            EnhancedStackTrace stackTrace,
            string title,
            params string[] messages)
        {
            this.Code = code;
            this.StackTrace = stackTrace;
            this.Messages = messages;
            this.Title = title;
        }

        /// <summary>
        /// Gets Title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Gets 堆栈.
        /// </summary>
        public new EnhancedStackTrace StackTrace { get; }

        /// <summary>
        /// Gets 消息.
        /// </summary>
        public string[] Messages { get; }
    }
}
