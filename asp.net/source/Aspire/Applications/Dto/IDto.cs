// <copyright file="IDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    /// <summary>
    /// 数据传输对象.
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键.</typeparam>
    public interface IDto<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets 主键.
        /// </summary>
        public TPrimaryKey Id { get; set; }
    }
}
