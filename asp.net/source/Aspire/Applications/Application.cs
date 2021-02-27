using Aspire.Exceptions;

using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace Aspire
{
    /// <summary>
    /// 应用程序
    /// </summary>
    [DynamicWebApi]
    [Authorize]
    public abstract class Application : IDynamicWebApi
    {
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="messages">错误编码</param>
        protected static T Failure<T>(params string[] messages)
        {
            FriendlyThrowException.ThrowException(messages);
            return default;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code">错误编码</param>
        /// <param name="messages">消息</param>
        protected static T Failure<T>(ResponseCode code, params string[] messages)
        {
            FriendlyThrowException.ThrowException(code, messages);
            return default;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code">错误编码</param>
        /// <param name="messages">消息</param>
        protected static T Failure<T>(int code, params string[] messages)
        {
            FriendlyThrowException.ThrowException(code, messages);
            return default;
        }
    }
}
