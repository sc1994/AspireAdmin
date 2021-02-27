using System;

namespace Aspire.Exceptions
{
    /// <summary>
    /// 友好异常
    /// </summary>
    internal class FriendlyException : Exception
    {
        public ResponseCode Code { get; }
        public string[] Messages { get; }

        public FriendlyException(ResponseCode code = ResponseCode.Failure, params string[] messages)
        {
            Code = code;
            Messages = messages;
        }
    }
}
