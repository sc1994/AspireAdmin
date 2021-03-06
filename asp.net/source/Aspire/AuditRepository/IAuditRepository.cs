using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Aspire
{
    /// <summary>
    /// 审计仓储 （主键为GUID）
    /// </summary>
    /// <typeparam name="TAuditEntity"></typeparam>
    public interface IAuditRepository<TAuditEntity> : IAuditRepository<TAuditEntity, Guid>
        where TAuditEntity : IAuditEntity
    {

    }

    /// <summary>
    /// 审计仓储
    /// </summary>
    /// <typeparam name="TAuditEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IAuditRepository<TAuditEntity, in TPrimaryKey>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
    {
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>成功与否</returns>
        public async Task<bool> InsertAsync(TAuditEntity entity)
        {
            return await this.InsertBatchAsync(new[] { entity }) == 1;
        }

        /// <summary>
        /// 增 然后 取得实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TAuditEntity> InsertThenEntityAsync(TAuditEntity entity);

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
        public Task<long> InsertBatchAsync(IEnumerable<TAuditEntity> entities)
        {
            return this.InsertBatchAsync(entities.ToArray());
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="primaryKey">指定主键</param>
        /// <returns>成功与否</returns>
        public async Task<bool> DeleteAsync(TPrimaryKey primaryKey)
        {
            return await this.DeleteBatchAsync(x => x.Id.Equals(primaryKey)) == 1;
        }

        /// <summary>
        /// 删 批量 
        /// </summary>
        /// <param name="primaryKeys">指定主键集合</param>
        /// <returns>影响行数</returns>
        public Task<long> DeleteBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return this.DeleteBatchAsync(x => primaryKeys.Contains(x.Id));
        }

        /// <summary>
        /// 删 批量 
        /// </summary>
        /// <param name="primaryKeys">指定主键集合</param>
        /// <returns>影响行数</returns>
        public Task<long> DeleteBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return this.DeleteBatchAsync(primaryKeys.ToArray());
        }

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
        public async Task<bool> UpdateAsync(TAuditEntity newEntity)
        {
            return await this.UpdateBatchAsync(new[] { newEntity }) == 1;
        }

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
        public Task<long> UpdateBatchAsync(IEnumerable<TAuditEntity> newEntities)
        {
            return this.UpdateBatchAsync(newEntities.ToArray());
        }

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns>数据库内容</returns>
        public Task<TAuditEntity> GetAsync(TPrimaryKey primaryKey)
        {
            return this.GetBatchAsync(x => x.Id.Equals(primaryKey), 1).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="primaryKeys">主键集合</param>
        /// <returns>数据库内容</returns>
        public Task<TAuditEntity[]> GetBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return this.GetBatchAsync(x => primaryKeys.Contains(x.Id), primaryKeys.Length);
        }

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="primaryKeys">主键集合</param>
        /// <returns>数据库内容</returns>
        Task<TAuditEntity[]> GetBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return this.GetBatchAsync(primaryKeys.ToArray());
        }

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

        /// <summary>
        /// 查 
        /// </summary>
        /// <param name="queryable">可查询对象 比如 ef 中的 IQueryable，freeSql 中的 ISelect</param>
        /// <param name="dto"></param>
        /// <returns>数据库内容</returns>
        Task<(TAuditEntity[] items, long totalCount)> PagingAsync(object queryable, PageInputDto dto);
    }
}