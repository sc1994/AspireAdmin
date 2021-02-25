namespace Aspire
{
    /// <summary>
    /// 当前登入用户
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; set; }
    }
}
