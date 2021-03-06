using System;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Aspire.Authenticate;

using FreeSql;

namespace Aspire.FreeSql.Provider
{
    internal class AuditRepository<TAuditEntity> : AuditRepository<TAuditEntity, Guid>
        where TAuditEntity : AuditEntity
    {
        public AuditRepository(IFreeSql freeSql, ICurrentUser currentUser) : base(freeSql, currentUser)
        {
        }
    }

    internal class AuditRepository<TAuditEntity, TPrimaryKey> : IAuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : AuditEntity<TPrimaryKey>
    {
        private readonly IFreeSql _freeSql;

        public ICurrentUser CurrentUser { get; }

        public AuditRepository(IFreeSql freeSql, ICurrentUser currentUser)
        {
            _freeSql = freeSql;
            CurrentUser = currentUser;
        }

        public async virtual Task<TAuditEntity> InsertThenEntityAsync(TAuditEntity entity)
        {
            SetCreatedEntity(ref entity);
            return await _freeSql.Insert(entity).ExecuteInsertedAsync().FirstOrDefaultAsync();
        }

        public async virtual Task<long> InsertBatchAsync(TAuditEntity[] entities)
        {
            entities.ForEach(x => SetCreatedEntity(ref x));
            return await _freeSql.Insert(entities).ExecuteAffrowsAsync();
        }

        public async virtual Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter)
        {
            return await _freeSql.Update<TAuditEntity>()
                .Where(filter)
                .Set(x => x.DeletedAt, DateTime.Now)
                .Set(x => x.DeletedUserName, CurrentUser.Name)
                .Set(x => x.DeletedUserAccount, CurrentUser.Account)
                .Set(x => x.Deleted, true)
                .ExecuteAffrowsAsync();
        }

        public async virtual Task<long> UpdateBatchAsync(TAuditEntity[] newEntities)
        {
            newEntities.ForEach(x => SetUpdatedEntity(ref x));
            return await _freeSql.Update<TAuditEntity>()
                .SetSource(newEntities)
                .ExecuteAffrowsAsync();
        }

        public async virtual Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter)
        {
            return await _freeSql
                .Select<TAuditEntity>()
                .Where(x => !x.Deleted)
                .Where(filter)
                .ToListAsync()
                .ToArrayAsync();
        }

        public async virtual Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit)
        {
            return await _freeSql
                .Select<TAuditEntity>()
                .Where(x => !x.Deleted)
                .Where(filter)
                .Take(int.Parse(limit.ToString())) // TODO 装拆箱
                .ToListAsync()
                .ToArrayAsync();
        }

        public async virtual Task<(TAuditEntity[] items, long totalCount)> PagingAsync(object queryable, PageInputDto dto)
        {
            if (queryable is ISelect<TAuditEntity> iSelect) {
                var itemsAsync = iSelect.ToListAsync<TAuditEntity>().ToArrayAsync();
                var totalCountAsync = iSelect.CountAsync();
                return (await itemsAsync, await totalCountAsync);
            }
            throw new ArgumentException($"参数{nameof(queryable)}应该为ISelect<{typeof(TAuditEntity).Name}>类型");
        }

        /// <summary>
        /// 设置 创建的审计实体 
        /// </summary>
        /// <param name="entity"></param>
        private void SetCreatedEntity(ref TAuditEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.CreatedUserName = CurrentUser.Name;
            entity.CreatedUserAccount = CurrentUser.Account;

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
        private void SetUpdatedEntity(ref TAuditEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedUserName = CurrentUser.Name;
            entity.UpdatedUserAccount = CurrentUser.Account;
        }
    }

    public static class FreeSqlExtension
    {
        public static ISelect<TAuditEntity> Select<TAuditEntity>(this IFreeSql freeSql)
            where TAuditEntity : AuditEntity
        {
            return freeSql.Select<TAuditEntity>().Where(x => !x.Deleted);
        }

        public static ISelect<TAuditEntity> Select<TAuditEntity, TPrimaryKey>(this IFreeSql freeSql)
            where TAuditEntity : AuditEntity<TPrimaryKey>
        {
            return freeSql.Select<TAuditEntity>().Where(x => !x.Deleted);
        }
    }
}
