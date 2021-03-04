namespace Aspire
{
    /// <summary>
    /// Jwt 配置项
    /// </summary>
    public class JwtAppSettings
    {
        /// <summary>
        /// Secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// 到期秒
        /// </summary>
        public int ExpireSeconds { get; set; }

        /// <summary>
        /// Header Key
        /// </summary>
        public string HeaderKey { get; set; }
    }
}