namespace Aspire
{
    /// <summary>
    /// Aspire 配置项
    /// </summary>
    public class AspireAppSettings
    {
        /// <summary>
        /// JWT 
        /// </summary>
        public JwtAppSettings Jwt { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public AdminAppSettings Administrator { get; set; }
    }
}
