namespace Aspire.Authorization
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
        /// 角色
        /// </summary>
        public string[] Roles { get; set; }
    }
}