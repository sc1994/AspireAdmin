// <copyright file="IAuditEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.AuditEntity
{
    using System;
    using System.ComponentModel;

    /// <inheritdoc />
    public interface IAuditEntity : IAuditEntity<Guid>
    {
        /// <summary>
        /// 初始化GUID.
        /// </summary>
        void InitId();
    }

    /// <summary>
    /// 审计实体.
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键.</typeparam>
    public interface IAuditEntity<TPrimaryKey> : IDto<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets 创建时间.
        /// </summary>
        [Description("创建时间")]
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets 创建人.
        /// </summary>
        [Description("创建人")]
        string CreatedUserName { get; set; }

        /// <summary>
        /// Gets or sets 创建人id.
        /// </summary>
        [Description("创建人id")]
        string CreatedUserAccount { get; set; }

        /// <summary>
        /// Gets or sets 更新时间.
        /// </summary>
        [Description("更新时间")]
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets 更新人.
        /// </summary>
        [Description("更新人")]
        string UpdatedUserName { get; set; }

        /// <summary>
        /// Gets or sets 更新人id.
        /// </summary>
        [Description("更新人id")]
        string UpdatedUserAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets 是否删除
        /// 使用仓储进行筛选无需筛选这个字段.
        /// </summary>
        [Description("是否删除")]
        bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets 删除时间.
        /// </summary>
        [Description("删除时间")]
        DateTime DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets 删除人.
        /// </summary>
        [Description("删除人")]
        string DeletedUserName { get; set; }

        /// <summary>
        /// Gets or sets 删除人id.
        /// </summary>
        [Description("删除人id")]
        string DeletedUserAccount { get; set; }
    }
}
