// <copyright file="LogItemsStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Aspire.Cache;

    /// <summary>
    /// Log Write Store.
    /// </summary>
    public class LogItemsStore
    {
        /// <summary>
        /// Redis Key Log Items.
        /// </summary>
        private const string RedisKeyByLogItems = "Aspire:Serilog:ElasticSearch:Select:RedisKeyByLogItems";

        /// <summary>
        /// Items Store.
        /// </summary>
        [SuppressMessage("Style", "IDE0090:使用 \"new(...)\"", Justification = "<挂起>")]
        private static readonly ConcurrentDictionary<string, byte> ItemsStore = new ConcurrentDictionary<string, byte>();

        private static int itemsCursor;
        private static bool isInitDone;
        private readonly IAspireCacheClient cacheClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogItemsStore"/> class.
        /// </summary>
        /// <param name="cacheClient">Cache Client.</param>
        public LogItemsStore(IAspireCacheClient cacheClient)
        {
            this.cacheClient = cacheClient;
        }

        /// <summary>
        /// Add Items.
        /// </summary>
        /// <param name="items">Items.</param>
        internal static void AddItems(object items)
        {
            if (isInitDone)
            {
                ItemsStore.TryAdd(items.SerializeObject(), 0);
            }
        }

        /// <summary>
        /// Get Items.
        /// </summary>
        /// <returns>Items.</returns>
        internal static string[] GetItems()
        {
            return ItemsStore.Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Clear Items Store.
        /// </summary>
        internal void ClearItemsStore()
        {
            this.cacheClient.DeleteKey(RedisKeyByLogItems);
            ItemsStore.Clear();
            itemsCursor = 0;
        }

        /// <summary>
        /// Init Items.
        /// </summary>
        internal void InitItems()
        {
            this.cacheClient
                .GetSetAllMembers(RedisKeyByLogItems)
                .ForEach(x =>
                {
                    ItemsStore.TryAdd(x, 0);
                });
            Console.WriteLine("初始化LogItemsStore长度: " + ItemsStore.Count);
            itemsCursor = ItemsStore.Count;
            isInitDone = true;

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(5000);

                    var length = ItemsStore.Count;
                    if (length <= itemsCursor)
                    {
                        continue;
                    }

                    Console.WriteLine($"消费起点: {itemsCursor}, 消费终点: {length}");

                    var items = ItemsStore
                        .Skip(itemsCursor)
                        .Take(length)
                        .Select(x => x.Key)
                        .ToArray();
                    this.cacheClient.AddSetMembers(RedisKeyByLogItems, items);
                    itemsCursor = length;
                }

                // ReSharper disable once FunctionNeverReturns
            });
        }
    }
}
