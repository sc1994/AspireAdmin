// <copyright file="TokenDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authorization
{
    using System;

    /// <summary>
    /// token 
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// BearerToken
        /// </summary>
        public string BearerToken { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpiryTime { get; set; }

        /// <summary>
        /// Ttl
        /// </summary>
        public int Ttl { get; set; }

        /// <summary>
        /// header key
        /// </summary>
        public string HeaderKey { get; set; }
    }
}
