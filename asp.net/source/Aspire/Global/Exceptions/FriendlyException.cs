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
        /// code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 堆栈.
        /// </summary>
        public new EnhancedStackTrace StackTrace { get; }

        /// <summary>
        /// 消息.
        /// </summary>
        public string[] Messages { get; }

        /// <summary>
        /// 实例化 异常.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="stackTrace"></param>
        /// <param name="messages"></param>
        public FriendlyException(
            int code,
            EnhancedStackTrace stackTrace,
            params string[] messages)
        {
            this.Code = code;
            this.StackTrace = stackTrace;
            this.Messages = messages;
        }
    }

    /// <summary>
    /// 友好的异常抛出
    /// </summary>
    public static class FriendlyThrowException
    {
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="messages"></param>
        public static void ThrowException(params string[] messages)
        {
            ThrowException(ResponseCode.InternalServerError, messages);
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="code"></param>
        /// <param name="messages"></param>
        public static void ThrowException(int code, params string[] messages)
        {
            throw new FriendlyException(code, EnhancedStackTrace.Current(), messages);
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="code"></param>
        /// <param name="messages"></param>
        public static void ThrowException(ResponseCode code, params string[] messages)
        {
            ThrowException(code.GetHashCode(), messages);
        }
    }
}
