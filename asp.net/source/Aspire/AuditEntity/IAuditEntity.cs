using System;

using Aspire.Dto;

namespace Aspire
{
    /// <summary>
    /// 审计实体
    /// </summary>
    public interface IAuditEntity : IAuditEntity<Guid>
    {
        /// <summary>
        /// 初始化GUID
        /// </summary>
        void InitId();
    }

    /// <summary>
    /// 审计实体
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IAuditEntity<TPrimaryKey> : IDto<TPrimaryKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        string CreatedUser { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        string CreatedUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        string UpdatedUser { get; set; }

        /// <summary>
        /// 更新人id
        /// </summary>
        string UpdatedUserId { get; set; }

        /// <summary>
        /// 是否删除
        /// 使用仓储进行筛选无需筛选这个字段
        /// </summary>
        bool Deleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime DeletedAt { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        string DeletedUser { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        string DeletedUserId { get; set; }
    }
}
