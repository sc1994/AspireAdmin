// <copyright file="FreeSqlAuditRepositoryOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.FreeSql.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlTypes;
    using System.Linq;
    using Aspire.AuditEntity;
    using Aspire.AuditRepository;
    using Aspire.Authenticate;
    using global::FreeSql;
    using global::FreeSql.Aop;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc/>
    public class FreeSqlAuditRepositoryOptionsSetup : IAuditRepositoryOptionsSetup
    {
        private readonly string connectionString;
        private readonly DataType dataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeSqlAuditRepositoryOptionsSetup"/> class.
        /// </summary>
        /// <param name="connectionString">Connection String.</param>
        /// <param name="dataType">Database Type.</param>
        public FreeSqlAuditRepositoryOptionsSetup(string connectionString, DataType dataType)
        {
            this.connectionString = connectionString;
            this.dataType = dataType;
        }

        /// <inheritdoc/>
        public void AddAuditRepository(IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var freeSql = new FreeSqlBuilder()
                .UseConnectionString(this.dataType, this.connectionString)
#if DEBUG
                .UseAutoSyncStructure(true) // 本地自动同步数据库
#endif
                .Build();

                freeSql.Aop.AuditValue += FreeSqlAopAuditValue;
                freeSql.Aop.CurdAfter += FreeSqlAopCurdAfter;
                freeSql.Aop.CurdBefore += FreeSqlAopCurdBefore;

                return freeSql;
            });

            services.AddScoped(typeof(IAuditRepository<>), typeof(AuditRepository<>));
            services.AddScoped(typeof(IAuditRepository<,>), typeof(AuditRepository<,>));
        }

        private static ICurrentUser GetCurrentUser()
        {
            return ServiceLocator.ServiceProvider.GetService<ICurrentUser>();
        }

        private static ILogWriter GetLogWriter()
        {
            try
            {
                var tmp = ServiceLocator.ServiceProvider.GetService<ILogWriter>();
                if (tmp is null)
                {
                    Console.WriteLine("Get ILogWriter Fail");
                }

                return tmp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }

        private static Dictionary<string, object> FormatDbParameters(IEnumerable<DbParameter> dbParameters)
        {
            return dbParameters
                .Select(x => new { x.ParameterName, x.Value })
                .ToDictionary(x => x.ParameterName, x => x.Value);
        }

        private static void FreeSqlAopAuditValue(object sender, AuditValueEventArgs e)
        {
            switch (e.AuditValueType)
            {
                case AuditValueType.Update:
                    e.Value = e.Property.Name switch
                    {
                        nameof(IAuditEntity.UpdatedAt)
                            when e.Value.Equals(SqlDateTime.MaxValue) || e.Value.Equals(default)
                            => DateTime.Now,
                        nameof(IAuditEntity.UpdatedUserAccount)
                            when e.Value is null || e.Value.ToString().IsNullOrWhiteSpace()
                            => GetCurrentUser().Account,
                        nameof(IAuditEntity.UpdatedUserName)
                            when e.Value is null || e.Value.ToString().IsNullOrWhiteSpace()
                            => GetCurrentUser().Name,
                        _ => e.Value
                    };

                    break;
                case AuditValueType.Insert:
                    e.Value = e.Property.Name switch
                    {
                        nameof(IAuditEntity.CreatedAt)
                            when e.Value.Equals(SqlDateTime.MaxValue) || e.Value.Equals(default)
                            => DateTime.Now,
                        nameof(IAuditEntity.CreatedUserAccount)
                            when e.Value is null || e.Value.ToString().IsNullOrWhiteSpace()
                            => GetCurrentUser().Account,
                        nameof(IAuditEntity.CreatedUserName)
                            when e.Value is null || e.Value.ToString().IsNullOrWhiteSpace()
                            => GetCurrentUser().Name,
                        _ => e.Value
                    };

                    break;
                case AuditValueType.InsertOrUpdate:
                    GetLogWriter()?.Warning("What Is FreeSql InsertOrUpdate", new
                    {
                        e.Property.Name,
                        e.Value,
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(e.AuditValueType.ToString());
            }
        }

        private static void FreeSqlAopCurdAfter(object sender, CurdAfterEventArgs e)
        {
            var msg = new
            {
                e.Sql,
                e.ElapsedMilliseconds,
                Exception = e.Exception?.ToString() ?? string.Empty,
                e.CurdType,
                DbParms = FormatDbParameters(e.DbParms),
            };
            GetLogWriter()?.Information("FreeSql Aop Curd After", msg, e.CurdType.ToString(), e.Table.CsName);
        }

        private static void FreeSqlAopCurdBefore(object sender, CurdBeforeEventArgs e)
        {
            if (e.CurdType != CurdType.Delete)
            {
                return;
            }

            var ex = new NotSupportedException("不允许进行物理数据删除操作");
            var msg = new
            {
                e.Sql,
                e.CurdType,
                DbParms = FormatDbParameters(e.DbParms),
            };
            GetLogWriter()?.Error(ex, msg, e.CurdType.ToString(), e.Table.CsName);
            throw ex; // 抛出不支持的操作异常
        }
    }
}