using System;
using System.Diagnostics;

namespace Aspire
{
    /// <summary>
    /// 友好异常
    /// </summary>
    internal class FriendlyException : Exception
    {
        public int Code { get; }
        public new EnhancedStackTrace StackTrace { get; }
        public string[] Messages { get; }

        public FriendlyException(
            int code,
            EnhancedStackTrace stackTrace,
            params string[] messages)
        {
            Code = code;
            StackTrace = stackTrace;
            Messages = messages;
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
