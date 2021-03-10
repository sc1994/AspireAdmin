// <copyright file="AuditRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.FreeSql.Provider
{
    using System;
    using System.Data.SqlTypes;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Aspire.AuditRepository;
    using Aspire.Authenticate;
    using global::FreeSql;

    /// <inheritdoc />
    internal class AuditRepository<TAuditEntity> : AuditRepository<TAuditEntity, Guid>
        where TAuditEntity : AuditEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRepository{TAuditEntity}"/> class.
        /// </summary>
        /// <param name="freeSql">FreeSql.</param>
        /// <param name="currentUser">CurrentUser.</param>
        public AuditRepository(IFreeSql freeSql, ICurrentUser currentUser)
            : base(freeSql, currentUser)
        {
        }
    }

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<挂起>")]
    internal class AuditRepository<TAuditEntity, TPrimaryKey> : IAuditRepository<TAuditEntity, TPrimaryKey>
        where TAuditEntity : AuditEntity<TPrimaryKey>
    {
        private readonly IFreeSql freeSql;

        private readonly ICurrentUser currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRepository{TAuditEntity, TPrimaryKey}"/> class.
        /// </summary>
        /// <param name="freeSql">FreeSql.</param>
        /// <param name="currentUser">CurrentUser.</param>
        public AuditRepository(IFreeSql freeSql, ICurrentUser currentUser)
        {
            this.freeSql = freeSql;
            this.currentUser = currentUser;
        }

        /// <inheritdoc />
        public virtual async Task<TAuditEntity> InsertThenEntityAsync(TAuditEntity entity)
        {
            this.SetCreatedEntity(ref entity);
            return await this.freeSql.Insert(entity).ExecuteInsertedAsync().FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public virtual async Task<long> InsertBatchAsync(TAuditEntity[] entities)
        {
            entities.ForEach(x => this.SetCreatedEntity(ref x));
            return await this.freeSql.Insert(entities).ExecuteAffrowsAsync();
        }

        /// <inheritdoc />
        public virtual async Task<long> DeleteBatchAsync(Expression<Func<TAuditEntity, bool>> filter)
        {
            return await this.freeSql.Update<TAuditEntity>()
                .Where(filter)
                .Set(x => x.DeletedAt, DateTime.Now)
                .Set(x => x.DeletedUserName, this.currentUser.Name)
                .Set(x => x.DeletedUserAccount, this.currentUser.Account)
                .Set(x => x.Deleted, true)
                .ExecuteAffrowsAsync();
        }

        /// <inheritdoc />
        public virtual async Task<long> UpdateBatchAsync(TAuditEntity[] newEntities)
        {
            newEntities.ForEach(x => this.SetUpdatedEntity(ref x));
            return await this.freeSql.Update<TAuditEntity>()
                .SetSource(newEntities)
                .ExecuteAffrowsAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter)
        {
            return await this.freeSql
                .Select<TAuditEntity>()
                .Where(x => !x.Deleted)
                .Where(filter)
                .ToListAsync()
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TAuditEntity[]> GetBatchAsync(Expression<Func<TAuditEntity, bool>> filter, long limit)
        {
            return await this.freeSql
                .Select<TAuditEntity>()
                .Where(x => !x.Deleted)
                .Where(filter)
                .Take(int.Parse(limit.ToString())) // TODO 装拆箱
                .ToListAsync()
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public virtual async Task<(TAuditEntity[] items, long totalCount)> PagingAsync(object queryable, PageInputDto dto)
        {
            if (queryable is ISelect<TAuditEntity> iSelect)
            {
                var itemsAsync = iSelect.ToListAsync<TAuditEntity>().ToArrayAsync();
                var totalCountAsync = iSelect.CountAsync();
                return (await itemsAsync, await totalCountAsync);
            }

            throw new ArgumentException($"参数{nameof(queryable)}应该为ISelect<{typeof(TAuditEntity).Name}>类型");
        }

        private void SetCreatedEntity(ref TAuditEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.CreatedUserName = this.currentUser.Name;
            entity.CreatedUserAccount = this.currentUser.Account;

            entity.Deleted = false;
            entity.DeletedAt = SqlDateTime.MaxValue.Value;
            entity.DeletedUserName = string.Empty;
            entity.DeletedUserAccount = string.Empty;

            entity.UpdatedAt = SqlDateTime.MaxValue.Value;
            entity.UpdatedUserName = string.Empty;
            entity.UpdatedUserAccount = string.Empty;
        }

        private void SetUpdatedEntity(ref TAuditEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedUserName = this.currentUser.Name;
            entity.UpdatedUserAccount = this.currentUser.Account;
        }
    }
}
