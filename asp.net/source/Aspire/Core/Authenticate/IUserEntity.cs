// <copyright file="IUserEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authenticate
{
    using System;
    using Aspire.AuditEntity;

    /// <summary>
    /// 用户 实体.
    /// </summary>
    public interface IUserEntity : IUserEntity<Guid>
    {
    }

    /// <summary>
    /// 用户 实体.
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键.</typeparam>
    public interface IUserEntity<TPrimaryKey> : ICurrentUser, IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets 密码.
        /// </summary>
        string Password { get; set; }
    }
}
