namespace Aspire
{
    /// <summary>
    /// 当前登入用户
    /// </summary>
    public interface ICurrentLoginUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; set; }
    }
}
