// <copyright file="ILoggerConfigure.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Logger
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Serilog Elastic Search Configure.
    /// </summary>
    public interface ILoggerConfigure
    {
        /// <summary>
        /// User Logger.
        /// </summary>
        /// <param name="app">IApplicationBuilder.</param>
        /// <returns>IApplicationBuilder .</returns>
        IApplicationBuilder UseLogger(IApplicationBuilder app);
    }
}
