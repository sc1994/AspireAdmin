// <copyright file="SystemLogSelectItemsDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.SystemLog
{
    /// <summary>
    /// 选项集合.
    /// </summary>
    public class SystemLogSelectItemsDto
    {
        /// <summary>
        /// Gets or sets API路由.
        /// </summary>
        public string[] ApiMethods { get; set; }

        /// <summary>
        /// Gets or sets API方法.
        /// </summary>
        public string[] ApiRouters { get; set; }

        /// <summary>
        /// Gets or sets 过滤1.
        /// </summary>
        public string[] Filter1s { get; set; }

        /// <summary>
        /// Gets or sets 过滤2.
        /// </summary>
        public string[] Filter2S { get; set; }

        /// <summary>
        /// Gets or sets 过滤3.
        /// </summary>
        public string[] Filter3s { get; set; }

        /// <summary>
        /// Gets or sets 过滤4.
        /// </summary>
        public string[] Filter4s { get; set; }

        /// <summary>
        /// Gets or sets 客户端地址.
        /// </summary>
        public string[] ClientAddress { get; set; }

        /// <summary>
        /// Gets or sets 服务端地址.
        /// </summary>
        public string[] ServerAddress { get; set; }
    }
}
