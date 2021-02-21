using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Aspire
{
    public interface IAuditRepository<TAuditEntity> : IAuditRepository<TAuditEntity, Guid>
        where TAuditEntity : IAuditEntity
    {

    }

    public interface IAuditRepository<TAuditEntity, in TPrimaryKey>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>成功与否</returns>
        Task<bool> InsertAsync(TAuditEntity entity);

        /// <summary>
        /// 增 批量 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>影响行数</returns>
        Task<long> InsertBatchAsync(TAuditEntity[] entities);

        /// <summary>
        /// 增 批量 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>影响行数</returns>
        Task<long> InsertBatchAsync(IEnumerable<TAuditEntity> entities);

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="primaryKey">指定主键</param>
        /// <returns>成功与否</returns>
        Task<bool> DeleteBatchAsync(TPrimaryKey primaryKey);

        /// <summary>
        /// 删 批量 
        /// </summary>
        /// <param name="primaryKeys">指定主键集合</param>
        /// <returns>影响行数</returns>
        Task<long> DeleteBatchAsync(TPrimaryKey[] primaryKeys);

        /// <summary>
        /// 删 批量 
        /// </summary>
        /// <param name="primaryKeys">指定主键集合</param>
        /// <returns>影响行数</returns>
        Task<long> DeleteBatchAsync(IEnumerable<TPrimaryKey> primaryKeys);

        /// <summary>
        /// 删 批量 
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>影响行数</returns>
        Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter);

        /// <summary>
        /// 改 
        /// </summary>
        /// <param name="newEntity">新实体</param>
        /// <returns>成功与否</returns>
        Task<bool> UpdateAsync(TAuditEntity newEntity);

        /// <summary>
        /// 改 批量 
        /// </summary>
        /// <param name="newEntities">新实体集合</param>
        /// <returns>影响行数</returns>
        Task<long> UpdateBatchAsync(TAuditEntity[] newEntities);

        /// <summary>
        /// 改 批量 
        /// </summary>
        /// <param name="newEntities">新实体集合</param>
        /// <returns>影响行数</returns>
        Task<long> UpdateBatchAsync(IEnumerable<TAuditEntity> newEntities);

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns>数据库内容</returns>
        Task<TAuditEntity> GetAsync(TPrimaryKey primaryKey);

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="primaryKeys">主键集合</param>
        /// <returns>数据库内容</returns>
        Task<TAuditEntity[]> GetBatchAsync(TPrimaryKey[] primaryKeys);

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="primaryKeys">主键集合</param>
        /// <returns>数据库内容</returns>
        Task<TAuditEntity[]> GetBatchAsync(IEnumerable<TPrimaryKey> primaryKeys);

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="filter">过滤</param>
        /// <returns>数据库内容</returns>
        Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter);

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="filter">过滤</param>
        /// <param name="limit"></param>
        /// <returns>数据库内容</returns>
        Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit);
    }
}