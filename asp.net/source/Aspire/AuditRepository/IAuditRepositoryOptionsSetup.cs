// <copyright file="IAuditRepositoryOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 审计仓储启动选项.
    /// </summary>
    public interface IAuditRepositoryOptionsSetup
    {
        /// <summary>
        /// 添加审计仓储.
        /// </summary>
        /// <param name="services">服务集.</param>
        void AddAuditRepository(IServiceCollection services);
    }
}
