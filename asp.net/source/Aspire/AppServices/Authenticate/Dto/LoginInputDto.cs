namespace Aspire.AppServices.Authenticate
{
    /// <summary>
    /// 登录 输入
    /// </summary>
    public class LoginInputDto
    {
        /// <summary>
        /// 用户 身份唯一标识
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}