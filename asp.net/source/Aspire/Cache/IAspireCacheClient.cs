// <copyright file="IAspireCacheClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Cache
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Cache.
    /// </summary>
    public interface IAspireCacheClient
    {
        /// <summary>
        /// Delete Key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Is Success.</returns>
        bool DeleteKey(string key);

        /// <summary>
        /// Contains Key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Is Exist.</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Get String.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Type Object.</returns>
        string GetString(string key);

        /// <summary>
        /// Set String.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="ttl">TTL.</param>
        /// <returns>Is Success.</returns>
        bool SetString(string key, string value, int ttl);

        /// <summary>
        /// Get String.
        /// </summary>
        /// <typeparam name="T">Json Deserialize Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <returns>Type Object.</returns>
        public T GetString<T>(string key)
        {
            return this.GetString(key).DeserializeObject<T>();
        }

        /// <summary>
        /// Set String.
        /// </summary>
        /// <typeparam name="T">Json Serialize Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <param name="ttl">TTL.</param>
        /// <returns>Is Success.</returns>
        public bool SetString<T>(string key, T value, int ttl)
        {
            return this.SetString(key, value.SerializeObject(), ttl);
        }

        /// <summary>
        /// Add Set Members.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="values">Value Array.</param>
        /// <returns>Is Success.</returns>
        bool AddSetMembers(string key, params string[] values);

        /// <summary>
        /// Add Set Members.
        /// </summary>
        /// <typeparam name="T">Json Serialize Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <param name="values">Value Array.</param>
        /// <returns>Is Success.</returns>
        public bool AddSetMembers<T>(string key, params T[] values)
        {
            return this.AddSetMembers(key, values.Select(x => x.SerializeObject()).ToArray());
        }

        /// <summary>
        /// Get Set All Members.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Member Array.</returns>
        IEnumerable<string> GetSetAllMembers(string key);

        /// <summary>
        /// Get Set All Members.
        /// </summary>
        /// <typeparam name="T">Json Deserialize Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <returns>Member Array.</returns>
        public IEnumerable<T> GetSetAllMembers<T>(string key)
        {
            return this.GetSetAllMembers(key).Select(x => x.DeserializeObject<T>());
        }
    }
}
