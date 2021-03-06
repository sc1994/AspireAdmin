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
        override public Guid Id { get; set; }
    }

    public class AuditEntity<TPrimaryKey> : IAuditEntity<TPrimaryKey>
    {
        /// <inheritdoc />
        [Column(IsIdentity = true, CanUpdate = false, IsPrimary = true)]
        virtual public TPrimaryKey Id { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false, CanUpdate = false)]
        virtual public DateTime CreatedAt { get; set; }
        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false, CanUpdate = false)]
        virtual public string CreatedUserName { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false, CanUpdate = false)]
        virtual public string CreatedUserAccount { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        virtual public DateTime UpdatedAt { get; set; }
        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false)]
        virtual public string UpdatedUserName { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        virtual public string UpdatedUserAccount { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        virtual public bool Deleted { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        virtual public DateTime DeletedAt { get; set; }
        /// <inheritdoc />
        [Column(StringLength = 50, IsNullable = false)]
        virtual public string DeletedUserName { get; set; }
        /// <inheritdoc />
        [Column(IsNullable = false)]
        virtual public string DeletedUserAccount { get; set; }
    }
}
