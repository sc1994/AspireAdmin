// <copyright file="LogWriteItemsStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using Aspire.Cache;

    /// <summary>
    /// Log Write Store.
    /// </summary>
    public class LogWriteItemsStore
    {
        private const string RedisKeyByLogItems = "Aspire:Serilog:ElasticSearch:Select:RedisKeyByLogItems";
        private static readonly ConcurrentDictionary<string, byte> ItemsStore = new ();
        private static bool lockConsume;

        private readonly IAspireCacheClient cacheClient;
        private readonly ILogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriteItemsStore"/> class.
        /// </summary>
        /// <param name="cacheClient">Cache Client.</param>
        /// <param name="logWriter">Log Writer.</param>
        public LogWriteItemsStore(IAspireCacheClient cacheClient, ILogWriter logWriter)
        {
            this.cacheClient = cacheClient;
            this.logWriter = logWriter;

            // 初始化字典
            this.cacheClient
                .GetSetAllMembers(RedisKeyByLogItems)
                .ForEach(x =>
                {
                    ItemsStore.TryAdd(x, 0);
                });

            Task.Run(async () =>
            {
                if (lockConsume)
                {
                    return;
                }

                lockConsume = true;
                while (true)
                {
                    await Task.Delay(5000); // 5s消费一次

                    this.Consume();
                }

                // ReSharper disable once FunctionNeverReturns
            });
        }

        /// <summary>
        /// Add Items.
        /// </summary>
        /// <param name="items">Items.</param>
        public static void AddItems(object items)
        {
            ItemsStore.TryAdd(items.SerializeObject(), 0);
        }

        /// <summary>
        /// Get Items.
        /// </summary>
        /// <returns>Items.</returns>
        public string[] GetItems()
        {
            return this.cacheClient.GetSetAllMembers(RedisKeyByLogItems).ToArray();
        }

        /// <summary>
        /// Delete Items.
        /// </summary>
        /// <returns>Is Success.</returns>
        public bool DeleteItems()
        {
            ItemsStore.Clear();
            return this.cacheClient.DeleteKey(RedisKeyByLogItems);
        }

        private void Consume()
        {
            var length = ItemsStore.Count;
            if (length > 0)
            {
                this.logWriter.Information("消费日志Items集合", "消费个数: " + length);
            }

            var items = ItemsStore.Take(length).Select(x => x.Key);
            this.cacheClient.AddSetMembers(RedisKeyByLogItems, items.ToArray());

            // 无需删除已经落入redis的集合
        }
    }
}
