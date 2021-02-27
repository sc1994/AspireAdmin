namespace Aspire.AppServices.Authenticate
{
    /// <summary>
    /// 注册
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 用户 身份唯一标识
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string UserRole { get; set; }
    }
}