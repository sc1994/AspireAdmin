// <copyright file="FriendlyThrowException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System.Diagnostics;

    /// <summary>
    /// 友好的异常抛出.
    /// </summary>
    public static class FriendlyThrowException
    {
        /// <summary>
        /// 抛出异常.
        /// </summary>
        /// <param name="messages">Messages.</param>
        public static void ThrowException(params string[] messages)
        {
            ThrowException(ResponseCode.InternalServerError, messages);
        }

        /// <summary>
        /// 抛出异常.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="title">Title.</param>
        /// <param name="messages">Messages.</param>
        public static void ThrowException(int code, string title, params string[] messages)
        {
            throw new FriendlyException(code, EnhancedStackTrace.Current(), title, messages);
        }

        /// <summary>
        /// 抛出异常.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="messages">Messages.</param>
        public static void ThrowException(ResponseCode code, params string[] messages)
        {
            ThrowException(code.GetHashCode(), code.GetDescription(), messages);
        }
    }
}
