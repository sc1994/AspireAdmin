// <copyright file="CacheClientRealize.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.CSRedis.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using Aspire.Cache;
    using global::CSRedis;

    /// <summary>
    /// Redis 实现.
    /// </summary>
    internal class CacheClientRealize : IAspireCacheClient
    {
        private readonly CSRedisClient redisClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheClientRealize"/> class.
        /// </summary>
        /// <param name="redisClient">CSRedis.RedisClient.</param>
        public CacheClientRealize(CSRedisClient redisClient)
        {
            this.redisClient = redisClient;
        }

        /// <inheritdoc/>
        public bool DeleteKey(string key)
        {
            return this.redisClient.Del(key) == 1;
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            return this.redisClient.Exists(key);
        }

        /// <inheritdoc/>
        public string GetString(string key)
        {
            return this.redisClient.Get(key);
        }

        /// <inheritdoc/>
        public bool SetString(string key, string value, int ttl)
        {
            return this.redisClient.Set(key, value, ttl);
        }

        /// <inheritdoc/>
        public bool AddSetMembers(string key, params string[] values)
        {
            return this.redisClient.SAdd(key, values.Select(x => (object)x).ToArray()) == values.Length;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetSetAllMembers(string key)
        {
            return this.redisClient.SMembers(key);
        }
    }
}
