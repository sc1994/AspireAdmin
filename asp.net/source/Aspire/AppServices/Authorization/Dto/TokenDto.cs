// <copyright file="TokenDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authorization
{
    using System;

    /// <summary>
    /// Token.
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// Gets or sets BearerToken.
        /// </summary>
        public string BearerToken { get; set; }

        /// <summary>
        /// Gets or sets 到期时间.
        /// </summary>
        public DateTime ExpiryTime { get; set; }

        /// <summary>
        /// Gets or sets Ttl.
        /// </summary>
        public int Ttl { get; set; }

        /// <summary>
        /// Gets or sets Header Key.
        /// </summary>
        public string HeaderKey { get; set; }
    }
}
