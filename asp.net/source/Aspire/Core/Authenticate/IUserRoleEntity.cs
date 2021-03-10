// <copyright file="IUserRoleEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authenticate
{
    using System;
    using Aspire.AuditEntity;

    /// <summary>
    /// 用户角色.
    /// </summary>
    public interface IUserRoleEntity : IUserRoleEntity<Guid>
    {
    }

    /// <summary>
    /// 用户角色.
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键.</typeparam>
    public interface IUserRoleEntity<TPrimaryKey> : IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets 角色名.
        /// </summary>
        string RoleName { get; set; }
    }
}