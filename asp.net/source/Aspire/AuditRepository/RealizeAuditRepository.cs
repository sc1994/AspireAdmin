using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Aspire
{
    /// <summary>
    /// 审计仓储 实现 部分自举 方法 默认GUID主键
    /// </summary>
    /// <typeparam name="TAuditEntity">审计实体</typeparam>
    public abstract class RealizeAuditRepository<TAuditEntity> : RealizeAuditRepository<TAuditEntity, Guid>
        where TAuditEntity : IAuditEntity
    {
        /// <summary>
        /// 审计仓储
        /// </summary>
        /// <param name="currentUser">当前登录用户</param>
        protected RealizeAuditRepository(ICurrentUser currentUser) : base(currentUser)
        {
        }
    }

    /// <summary>
    /// 审计仓储 实现 部分自举 方法
    /// </summary>
    /// <typeparam name="TAuditEntity">审计实体</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public abstract class RealizeAuditRepository<TAuditEntity, TPrimaryKey> : IAuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 审计仓储
        /// </summary>
        /// <param name="currentUser">当前登录用户</param>
        protected RealizeAuditRepository(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        /// <inheritdoc />
        public virtual async Task<bool> InsertAsync(TAuditEntity entity)
        {
            return await InsertBatchAsync(new[] { entity }) == 1;
        }
        /// <inheritdoc />
        public abstract Task<TAuditEntity> InsertThenEntityAsync(TAuditEntity entity);
        /// <inheritdoc />
        public abstract Task<long> InsertBatchAsync(TAuditEntity[] entities);
        /// <inheritdoc />
        public virtual Task<long> InsertBatchAsync(IEnumerable<TAuditEntity> entities)
        {
            return InsertBatchAsync(entities.ToArray());
        }
        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(TPrimaryKey primaryKey)
        {
            return await DeleteBatchAsync(x => x.Id.Equals(primaryKey)) == 1;
        }
        /// <inheritdoc />
        public virtual Task<long> DeleteBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return DeleteBatchAsync(x => primaryKeys.Contains(x.Id));
        }
        /// <inheritdoc />
        public virtual Task<long> DeleteBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return DeleteBatchAsync(primaryKeys.ToArray());
        }
        /// <inheritdoc />
        public abstract Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter);
        /// <inheritdoc />
        public virtual async Task<bool> UpdateAsync(TAuditEntity newEntity)
        {
            return await UpdateBatchAsync(new[] { newEntity }) == 1;
        }
        /// <inheritdoc />
        public abstract Task<long> UpdateBatchAsync(TAuditEntity[] newEntities);
        /// <inheritdoc />
        public virtual Task<long> UpdateBatchAsync(IEnumerable<TAuditEntity> newEntities)
        {
            return UpdateBatchAsync(newEntities.ToArray());
        }
        /// <inheritdoc />
        public virtual Task<TAuditEntity> GetAsync(TPrimaryKey primaryKey)
        {
            return GetBatchAsync(x => x.Id.Equals(primaryKey), 1).FirstOrDefaultAsync();
        }
        /// <inheritdoc />
        public virtual Task<TAuditEntity[]> GetBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return GetBatchAsync(x => primaryKeys.Contains(x.Id), primaryKeys.Length);
        }
        /// <inheritdoc />
        public virtual Task<TAuditEntity[]> GetBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return GetBatchAsync(primaryKeys.ToArray());
        }
        /// <inheritdoc />
        public abstract Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter);
        /// <inheritdoc />
        public abstract Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit);
        /// <inheritdoc />
        public abstract Task<(TAuditEntity[] items, long totalCount)> PagingAsync(object queryable, PageInputDto dto);

        /// <summary>
        /// 设置 创建的审计实体 
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetCreatedEntity(ref TAuditEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.CreatedUserName = _currentUser.Name;
            entity.CreatedUserAccount = _currentUser.Account;

            entity.Deleted = false;
            entity.DeletedAt = SqlDateTime.MaxValue.Value;
            entity.DeletedUserName = string.Empty;
            entity.DeletedUserAccount = string.Empty;

            entity.UpdatedAt = SqlDateTime.MaxValue.Value;
            entity.UpdatedUserName = string.Empty;
            entity.UpdatedUserAccount = string.Empty;
        }

        /// <summary>
        /// 设置 更新的审计实体 
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetUpdatedEntity(ref TAuditEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedUserName = _currentUser.Name;
            entity.UpdatedUserAccount = _currentUser.Account;
        }
    }
}