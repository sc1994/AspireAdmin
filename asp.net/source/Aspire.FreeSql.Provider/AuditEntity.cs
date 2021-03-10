// <copyright file="AuditEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.FreeSql.Provider
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Aspire.AuditEntity;
    using global::FreeSql.DataAnnotations;

    /// <inheritdoc cref="AuditEntity{Guid}" />
    public class AuditEntity : AuditEntity<Guid>, IAuditEntity
    {
        /// <inheritdoc />
        [Column(CanUpdate = false, IsPrimary = true)]
        public override Guid Id { get; set; }

        /// <inheritdoc />
        public void InitId()
        {
            this.Id = GuidUtility.NewOrderlyGuid();
        }
    }

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    public class AuditEntity<TPrimaryKey> : IAuditEntity<TPrimaryKey>
    {
        /// <inheritdoc />
        [Column(IsIdentity = true, CanUpdate = false, IsPrimary = true)]
        public virtual TPrimaryKey Id { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false, CanUpdate = false)]
        public virtual DateTime CreatedAt { get; set; }

        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false, CanUpdate = false)]
        public virtual string CreatedUserName { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false, CanUpdate = false)]
        public virtual string CreatedUserAccount { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual DateTime UpdatedAt { get; set; }

        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false)]
        public virtual string UpdatedUserName { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual string UpdatedUserAccount { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual bool Deleted { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual DateTime DeletedAt { get; set; }

        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false)]
        public virtual string DeletedUserName { get; set; }

        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual string DeletedUserAccount { get; set; }
    }
}
