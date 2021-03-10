// <copyright file="GuidUtility.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;

    /// <summary>
    /// GUID 工具.
    /// </summary>
    public static class GuidUtility
    {
        /// <summary>
        /// 创建新的有序GUID.
        /// </summary>
        /// <returns>GUID.</returns>
        public static Guid NewOrderlyGuid()
        {
            return Guid.NewGuid(); // TODO 有序的GUID
        }
    }
}
