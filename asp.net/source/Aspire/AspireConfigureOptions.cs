namespace Aspire
{
    /// <summary>
    /// Aspire 配置项
    /// </summary>
    public class AspireConfigureOptions
    {
        /// <summary>
        /// JWT Secret
        /// </summary>
        public string JwtSecret { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public AdministratorOptions Administrator { get; set; }
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
        public string UserId { get; set; }

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
