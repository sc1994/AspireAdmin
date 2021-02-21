using System;

using FreeSql.DataAnnotations;

namespace Aspire.FreeSql.Provider
{
    public class AuditEntity : AuditEntity<Guid>, IAuditEntity
    {
        public void InitId()
        {
            Id = GuidUtility.NewOrderlyGuid();
        }

        [Column(CanUpdate = false, IsPrimary = true)]
        public override Guid Id { get; set; }
    }

    public class AuditEntity<TPrimaryKey> : IAuditEntity<TPrimaryKey>
    {
        [Column(IsIdentity = true, CanUpdate = false, IsPrimary = true)]
        public virtual TPrimaryKey Id { get; set; }
        [Column(IsNullable = false, CanUpdate = false)]
        public virtual DateTime CreatedAt { get; set; }
        [Column(StringLength = 50, IsNullable = false, CanUpdate = false)]
        public virtual string CreatedUser { get; set; }
        [Column(IsNullable = false, CanUpdate = false)]
        public virtual string CreatedUserId { get; set; }
        [Column(IsNullable = false)]
        public virtual DateTime UpdatedAt { get; set; }
        [Column(StringLength = 50, IsNullable = false)]
        public virtual string UpdatedUser { get; set; }
        [Column(IsNullable = false)]
        public virtual string UpdatedUserId { get; set; }
        [Column(IsNullable = false)]
        public virtual bool Deleted { get; set; }
        [Column(IsNullable = false)]
        public virtual DateTime DeletedAt { get; set; }
        [Column(StringLength = 50, IsNullable = false)]
        public virtual string DeletedUser { get; set; }
        [Column(IsNullable = false)]
        public virtual string DeletedUserId { get; set; }
    }
}
