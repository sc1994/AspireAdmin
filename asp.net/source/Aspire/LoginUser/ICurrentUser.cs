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
        string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        string Name { get; set; }
    }
}
