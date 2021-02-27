using System;

namespace Aspire.Authenticate
{
    /// <summary>
    /// 用户 实体
    /// </summary>
    public interface IUserEntity : IUserEntity<Guid>
    {
    }

    /// <summary>
    /// 用户 实体
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IUserEntity<TPrimaryKey> : ICurrentUser, IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }
    }
}
