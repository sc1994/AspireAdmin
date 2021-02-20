using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

// ReSharper disable ArrangeModifiersOrder

namespace Aspire
{
    public abstract class AuditRepository<TAuditEntity> : AuditRepository<TAuditEntity, Guid>
        where TAuditEntity : IAuditEntity
    {
        protected AuditRepository(ILoginUser loginUser) : base(loginUser)
        {
        }
    }

    public abstract class AuditRepository<TAuditEntity, TPrimaryKey> : IAuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : IAuditEntity<TPrimaryKey>
    {
        private readonly ILoginUser _loginUser;

        protected AuditRepository(ILoginUser loginUser)
        {
            _loginUser = loginUser;
        }

        public virtual async Task<bool> InsertAsync(TAuditEntity entity)
        {
            return await InsertBatchAsync(new[] { entity }) == 1;
        }
        public abstract Task<long> InsertBatchAsync(TAuditEntity[] entities);
        public virtual Task<long> InsertBatchAsync(IEnumerable<TAuditEntity> entities)
        {
            return InsertBatchAsync(entities.ToArray());
        }
        public virtual async Task<bool> DeleteBatchAsync(TPrimaryKey primaryKey)
        {
            return await DeleteBatchAsync(x => x.Id.Equals(primaryKey)) == 1;
        }
        public virtual Task<long> DeleteBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return DeleteBatchAsync(x => primaryKeys.Contains(x.Id));
        }
        public virtual Task<long> DeleteBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return DeleteBatchAsync(primaryKeys.ToArray());
        }
        public abstract Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter);
        public virtual async Task<bool> UpdateAsync(TAuditEntity newEntity)
        {
            return await UpdateBatchAsync(new[] { newEntity }) == 1;
        }
        public abstract Task<long> UpdateBatchAsync(TAuditEntity[] newEntities);
        public virtual Task<long> UpdateBatchAsync(IEnumerable<TAuditEntity> newEntities)
        {
            return UpdateBatchAsync(newEntities.ToArray());
        }
        public virtual Task<TAuditEntity> GetAsync(TPrimaryKey primaryKey)
        {
            return GetBatchAsync(x => x.Id.Equals(primaryKey), 1).FirstOrDefaultAsync();
        }
        public virtual Task<TAuditEntity[]> GetBatchAsync(TPrimaryKey[] primaryKeys)
        {
            return GetBatchAsync(x => primaryKeys.Contains(x.Id), primaryKeys.Length);
        }
        public virtual Task<TAuditEntity[]> GetBatchAsync(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return GetBatchAsync(primaryKeys.ToArray());
        }
        public abstract Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter);
        public abstract Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit);

        protected virtual void SetCreatedEntity(ref TAuditEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.CreatedUser = _loginUser.UserName;
            entity.CreatedUserId = _loginUser.UserId;

            entity.Deleted = false;
            entity.DeletedAt = SqlDateTime.MaxValue.Value;
            entity.DeletedUser = string.Empty;
            entity.DeletedUserId = string.Empty;

            entity.UpdatedAt = SqlDateTime.MaxValue.Value;
            entity.UpdatedUser = string.Empty;
            entity.UpdatedUserId = string.Empty;
        }

        protected virtual void SetUpdatedEntity(ref TAuditEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedUser = _loginUser.UserName;
            entity.UpdatedUserId = _loginUser.UserId;
        }
    }
}