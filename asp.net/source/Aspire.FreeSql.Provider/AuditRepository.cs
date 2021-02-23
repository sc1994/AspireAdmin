using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Aspire.Dto;

using FreeSql;

namespace Aspire.FreeSql.Provider
{
    public class AuditRepository<TAuditEntity> : AuditRepository<TAuditEntity, Guid>
        where TAuditEntity : AuditEntity
    {
        public AuditRepository(IFreeSql freeSql, ICurrentLoginUser currentLoginUser) : base(freeSql, currentLoginUser)
        {
        }
    }

    public class AuditRepository<TAuditEntity, TPrimaryKey> : RealizeAuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : AuditEntity<TPrimaryKey>
    {
        private readonly IFreeSql _freeSql;
        private readonly ICurrentLoginUser _currentLoginUser;


        public AuditRepository(IFreeSql freeSql, ICurrentLoginUser currentLoginUser) : base(currentLoginUser)
        {
            _freeSql = freeSql;
            _currentLoginUser = currentLoginUser;
        }

        public async override Task<TAuditEntity> InsertThenEntityAsync(TAuditEntity entity)
        {
            SetCreatedEntity(ref entity);
            return await _freeSql.Insert(entity).ExecuteInsertedAsync().FirstOrDefaultAsync();
        }

        public async override Task<long> InsertBatchAsync(TAuditEntity[] entities)
        {
            entities.ForEach(x => SetCreatedEntity(ref x));
            return await _freeSql.Insert(entities).ExecuteAffrowsAsync();
        }

        public async override Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter)
        {
            return await _freeSql.Update<TAuditEntity>()
                .Where(filter)
                .Set(x => x.DeletedAt, DateTime.Now)
                .Set(x => x.DeletedUser, _currentLoginUser.UserName)
                .Set(x => x.DeletedUserId, _currentLoginUser.UserId)
                .Set(x => x.Deleted, true)
                .ExecuteAffrowsAsync();
        }

        public async override Task<long> UpdateBatchAsync(TAuditEntity[] newEntities)
        {
            newEntities.ForEach(x => SetUpdatedEntity(ref x));
            return await _freeSql.Update<TAuditEntity>()
                .SetSource(newEntities)
                .ExecuteAffrowsAsync();
        }

        public async override Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter)
        {
            return await _freeSql
                .Select<TAuditEntity>()
                .Where(x => !x.Deleted)
                .Where(filter)
                .ToListAsync()
                .ToArrayAsync();
        }

        public async override Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit)
        {
            return await _freeSql
                .Select<TAuditEntity>()
                .Where(x => !x.Deleted)
                .Where(filter)
                .Take(int.Parse(limit.ToString())) // TODO 装拆箱
                .ToListAsync()
                .ToArrayAsync();
        }

        public async override Task<(TAuditEntity[] items, long totalCount)> PagingAsync(object queryable, PageInputDto dto)
        {
            if (queryable is ISelect<TAuditEntity> iSelect) {
                var itemsAsync = iSelect.ToListAsync<TAuditEntity>().ToArrayAsync();
                var totalCountAsync = iSelect.CountAsync();
                return (await itemsAsync, await totalCountAsync);
            }
            throw new ArgumentException($"参数{nameof(queryable)}应该为ISelect<TAuditEntity>类型");
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
