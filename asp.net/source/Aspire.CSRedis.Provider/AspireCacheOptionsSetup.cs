// <copyright file="AspireCacheOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.CSRedis.Provider
{
    using Aspire.Cache;
    using global::CSRedis;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class AspireCacheOptionsSetup : IAspireRedisOptionsSetup
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspireCacheOptionsSetup"/> class.
        /// </summary>
        /// <param name="connectionString">Connection String.</param>
        public AspireCacheOptionsSetup(string connectionString)
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
