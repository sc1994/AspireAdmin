// <copyright file="ILoggerOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Logger
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// logger 启动配置
    /// </summary>
    public interface ILoggerOptionsSetup
    {
        /// <summary>
        /// add logger 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        void AddLogger(IServiceCollection services, IConfiguration configuration);
    }
}
