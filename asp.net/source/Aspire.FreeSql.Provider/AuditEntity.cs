using System;
using FreeSql.DataAnnotations;

namespace Aspire.FreeSql.Provider
{
    public class AuditEntity : AuditEntity<Guid>, IAuditEntity
    {
        /// <inheritdoc />
        public void InitId()
        {
            this.Id = GuidUtility.NewOrderlyGuid();
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
