// <copyright file="PagedResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 分页结果.
    /// </summary>
    /// <typeparam name="TItem">item.</typeparam>
    public class PagedResultDto<TItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResultDto{TItem}"/> class.
        /// </summary>
        /// <param name="items">items.</param>
        /// <param name="totalCount">total count.</param>
        public PagedResultDto(IEnumerable<TItem> items, long totalCount)
        {
            this.Items = items.ToList();
            this.TotalCount = totalCount;
        }

        /// <summary>
        /// Gets items.
        /// </summary>
        public List<TItem> Items { get; }

        /// <summary>
        /// Gets total count.
        /// </summary>
        public long TotalCount { get; }
    }
}
