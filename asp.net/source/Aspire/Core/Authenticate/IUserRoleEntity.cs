using System;

namespace Aspire.Core.Authenticate
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public interface IUserRoleEntity : IUserRoleEntity<Guid>
    {

    }

    /// <summary>
    /// 用户角色
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IUserRoleEntity<TPrimaryKey> : IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// 路由名
        /// </summary>
        public string RoleName { get; set; }
    }
}