using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

// ReSharper disable ArrangeModifiersOrder

namespace Aspire.FreeSql.Provider
{
    public class FreeSqlAuditRepository<TAuditEntity> : FreeSqlAuditRepository<TAuditEntity, Guid>
        where TAuditEntity : FreeAuditEntity
    {
        public FreeSqlAuditRepository(IFreeSql freeSql, ILoginUser loginUser) : base(freeSql, loginUser)
        {
        }
    }

    public class FreeSqlAuditRepository<TAuditEntity, TPrimaryKey> : AuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : FreeAuditEntity<TPrimaryKey>
    {
        private readonly IFreeSql _freeSql;
        private readonly ILoginUser _loginUser;

        public FreeSqlAuditRepository(IFreeSql freeSql, ILoginUser loginUser) : base(loginUser)
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

    public static class xxxx<TAuditEntity>
    {
        public static void xx<TAuditEntity>(this IAuditRepository<TAuditEntity> xxx)
        {

        }
    }
}
