namespace Aspire.AppServices.Authenticate
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
    }
}