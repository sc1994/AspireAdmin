namespace Aspire
{
    /// <summary>
    /// Aspire 配置项
    /// </summary>
    public class AspireConfigureOptions
    {
        /// <summary>
        /// JWT 
        /// </summary>
        public JwtOptions Jwt { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public AdministratorOptions Administrator { get; set; }
    }

    /// <summary>
    /// Jwt 配置项
    /// </summary>
    public class JwtOptions
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

    /// <summary>
    /// 管理员 配置项
    /// </summary>
    public class AdministratorOptions
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
