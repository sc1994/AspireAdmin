// <copyright file="IAspireRedisOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Cache
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Aspire Redis Options Setup.
    /// </summary>
    public interface IAspireRedisOptionsSetup
    {
        /// <summary>
        /// Add Aspire Redis.
        /// </summary>
        /// <param name="service">Service Collection.</param>
        void AddAspireRedis(IServiceCollection service);
    }
}
