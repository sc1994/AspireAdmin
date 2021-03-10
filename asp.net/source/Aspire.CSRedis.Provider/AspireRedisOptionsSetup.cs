// <copyright file="AspireRedisOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.CSRedis.Provider
{
    using Aspire.Cache;
    using global::CSRedis;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class AspireRedisOptionsSetup : IAspireRedisOptionsSetup
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspireRedisOptionsSetup"/> class.
        /// </summary>
        /// <param name="connectionString">Connection String.</param>
        public AspireRedisOptionsSetup(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <inheritdoc />
        public void AddAspireRedis(IServiceCollection service)
        {
            service.AddSingleton<IAspireCacheClient, CacheClientRealize>();
            service.AddSingleton(serviceProvider => new CSRedisClient(this.connectionString));
        }
    }
}
