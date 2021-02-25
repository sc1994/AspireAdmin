using System.Collections.Generic;
using System.Linq;

namespace Aspire
{
    /// <summary>
    /// 分页后结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// 实例 分页后结果 构造
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        public PagedResultDto(IEnumerable<T> items, long totalCount)
        {
            Items = items.ToList();
            TotalCount = totalCount;
        }

        /// <summary>
        /// 项
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// 总数量
        /// </summary>
        public long TotalCount { get; }
    }
}
