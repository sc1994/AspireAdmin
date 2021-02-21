using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using FreeSql;

namespace Aspire.FreeSql.Provider
{
    public class AuditRepository<TAuditEntity> : AuditRepository<TAuditEntity, Guid>
        where TAuditEntity : AuditEntity
    {
        public AuditRepository(IFreeSql freeSql, ILoginUser loginUser) : base(freeSql, loginUser)
        {
        }
    }

    public class AuditRepository<TAuditEntity, TPrimaryKey> : BlankAuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : AuditEntity<TPrimaryKey>
    {
        private readonly IFreeSql _freeSql;
        private readonly ILoginUser _loginUser;

        public AuditRepository(IFreeSql freeSql, ILoginUser loginUser) : base(loginUser)
        {
            _freeSql = freeSql;
            _loginUser = loginUser;
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
                .Set(x => x.DeletedUser, _loginUser.UserName)
                .Set(x => x.DeletedUserId, _loginUser.UserId)
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
