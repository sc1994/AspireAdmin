// <copyright file="FreeSqlExtension.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.FreeSql.Provider
{
    using System;
    using global::FreeSql;

    /// <summary>
    /// FreeSql Extension.
    /// </summary>
    public static class FreeSqlExtension
    {
        /// <summary>
        /// Select.
        /// </summary>
        /// <typeparam name="TAuditEntity">Audit Entity.</typeparam>
        /// <param name="freeSql">freeSql.</param>
        /// <returns>Audit Entity Object.</returns>
        public static ISelect<TAuditEntity> Select<TAuditEntity>(this IFreeSql freeSql)
            where TAuditEntity : AuditEntity
        {
            return Select<TAuditEntity, Guid>(freeSql);
        }

        /// <summary>
        /// Select.
        /// </summary>
        /// <typeparam name="TAuditEntity">Audit Entity.</typeparam>
        /// <typeparam name="TPrimaryKey">Primary Key.</typeparam>
        /// <param name="freeSql">freeSql.</param>
        /// <returns>Audit Entity Object.</returns>
        public static ISelect<TAuditEntity> Select<TAuditEntity, TPrimaryKey>(this IFreeSql freeSql)
            where TAuditEntity : AuditEntity<TPrimaryKey>
        {
            return freeSql.Select<TAuditEntity>().Where(x => !x.Deleted);
        }
    }
}
