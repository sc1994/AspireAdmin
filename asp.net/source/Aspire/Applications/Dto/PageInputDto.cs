// <copyright file="PageInputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 页输入.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "<挂起>")]
    public interface IPageInputDto
    {
        /// <summary>
        /// Gets or sets 页 索引.
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets 页 大小.
        /// </summary>
        int PageSize { get; set; }
    }

    /// <inheritdoc />
    public class PageInputDto : IPageInputDto
    {
        /// <inheritdoc />
        public int PageIndex { get; set; } = 1;

        /// <inheritdoc />
        public int PageSize { get; set; } = 10;
    }
}
