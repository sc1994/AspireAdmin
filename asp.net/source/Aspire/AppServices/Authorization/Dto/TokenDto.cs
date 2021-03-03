using System;

namespace Aspire.Authorization
{
    /// <summary>
    /// token 
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// BearerToken
        /// </summary>
        public string BearerToken { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpiryTime { get; set; }

        /// <summary>
        /// Ttl
        /// </summary>
        public int Ttl { get; set; }

        /// <summary>
        /// header key
        /// </summary>
        public string HeaderKey { get; set; }
    }
}
