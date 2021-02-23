using System;

using FreeSql.DataAnnotations;

namespace Aspire.FreeSql.Provider
{
    public class AuditEntity : AuditEntity<Guid>, IAuditEntity
    {
        /// <inheritdoc />
        public void InitId()
        {
            Id = GuidUtility.NewOrderlyGuid();
        }

        /// <inheritdoc />
        [Column(CanUpdate = false, IsPrimary = true)]
        public override Guid Id { get; set; }
    }

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
        public virtual string CreatedUser { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false, CanUpdate = false)]
        public virtual string CreatedUserId { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual DateTime UpdatedAt { get; set; }
        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false)]
        public virtual string UpdatedUser { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual string UpdatedUserId { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual bool Deleted { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual DateTime DeletedAt { get; set; }
        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false)]
        public virtual string DeletedUser { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        public virtual string DeletedUserId { get; set; }
    }
}
