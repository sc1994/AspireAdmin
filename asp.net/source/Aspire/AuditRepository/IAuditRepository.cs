// <copyright file="IAuditRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 审计仓储 （主键为GUID）.
    /// </summary>
    /// <typeparam name="TAuditEntity">Audit Entity.</typeparam>
    public interface IAuditRepository<TAuditEntity> : IAuditRepository<TAuditEntity, Guid>
        where TAuditEntity : IAuditEntity
    {
    }

    /// <summary>
    /// 审计仓储.
    /// </summary>
    /// <typeparam name="TAuditEntity">Audit Entity.</typeparam>
    /// <typeparam name="TPrimaryKey">Primary Key.</typeparam>
    public interface IAuditRepository<TAuditEntity, in TPrimaryKey>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="entity">Audit Entity.</param>
        /// <returns>成功与否.</returns>
        public async Task<bool> InsertAsync(TAuditEntity entity)
        {
            return await this.InsertBatchAsync(new[] { entity }) == 1;
        }

        /// <summary>
        /// Insert Then Entity.
        /// </summary>
        /// <param name="entity">Audit Entity.</param>
        /// <returns>Entity.</returns>
        Task<TAuditEntity> InsertThenEntityAsync(TAuditEntity entity);

        /// <summary>
        /// Insert Batch.
        /// </summary>
        /// <param name="entities">Audit Entity Array.</param>
        /// <returns>影响行数.</returns>
        Task<long> InsertBatchAsync(TAuditEntity[] entities);

        /// <summary>
        /// Insert Batch.
        /// </summary>
        /// <param name="entities">Audit Entity Array.</param>
        /// <returns>影响行数.</returns>
        public Task<long> InsertBatchAsync(IEnumerable<TAuditEntity> entities)
        {
            return this.InsertBatchAsync(entities.ToArray());
        }

        /// <summary>
        /// Delete.
        /// </summary>
        /// <param name="primaryKey">Primary Key.</param>
        /// <returns>成功与否.</returns>
        public async Task<bool> DeleteAsync(TPrimaryKey primaryKey)
        {
            return await this.DeleteBatchAsync(x => x.Id.Equals(primaryKey)) == 1;
        }

        /// <summary>
        /// Delete Batch.
        /// </summary>
        /// <param name="primaryKeys">Primary Key Array.</param>
        /// <returns>影响行数.</returns>
        public Task<long> DeleteBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return this.DeleteBatchAsync(x => primaryKeys.Contains(x.Id));
        }

        /// <summary>
        /// Delete Batch.
        /// </summary>
        /// <param name="primaryKeys">Primary Key Array.</param>
        /// <returns>影响行数.</returns>
        public Task<long> DeleteBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return this.DeleteBatchAsync(primaryKeys.ToArray());
        }

        /// <summary>
        /// Delete Batch.
        /// </summary>
        /// <param name="filter">过滤条件.</param>
        /// <returns>影响行数.</returns>
        Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter);

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="newEntity">New Entity.</param>
        /// <returns>成功与否.</returns>
        public async Task<bool> UpdateAsync(TAuditEntity newEntity)
        {
            return await this.UpdateBatchAsync(new[] { newEntity }) == 1;
        }

        /// <summary>
        /// Update Batch.
        /// </summary>
        /// <param name="newEntities">New Entitiy Array.</param>
        /// <returns>影响行数.</returns>
        Task<long> UpdateBatchAsync(TAuditEntity[] newEntities);

        /// <summary>
        /// Update Batch.
        /// </summary>
        /// <param name="newEntities">New Entitiy Array.</param>
        /// <returns>影响行数.</returns>
        public Task<long> UpdateBatchAsync(IEnumerable<TAuditEntity> newEntities)
        {
            return this.UpdateBatchAsync(newEntities.ToArray());
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="primaryKey">Primary Key.</param>
        /// <returns>数据库内容.</returns>
        public Task<TAuditEntity> GetAsync(TPrimaryKey primaryKey)
        {
            return this.GetBatchAsync(x => x.Id.Equals(primaryKey), 1).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="primaryKeys">Primary Key Array.</param>
        /// <returns>数据库内容.</returns>
        public Task<TAuditEntity[]> GetBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return this.GetBatchAsync(x => primaryKeys.Contains(x.Id), primaryKeys.Length);
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="primaryKeys">Primary Key Array.</param>
        /// <returns>数据库内容.</returns>
        Task<TAuditEntity[]> GetBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return this.GetBatchAsync(primaryKeys.ToArray());
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="filter">Filter.</param>
        /// <returns>数据库内容.</returns>
        Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter);

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="filter">Filter.</param>
        /// <param name="limit">Limit.</param>
        /// <returns>数据库内容.</returns>
        Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit);

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="queryable">可查询对象 比如 ef 中的 IQueryable，freeSql 中的 ISelect.</param>
        /// <param name="dto">Page Input.</param>
        /// <returns>数据库内容.</returns>
        Task<(TAuditEntity[] items, long totalCount)> PagingAsync(object queryable, PageInputDto dto);
    }
}