// <copyright file="JwtAppSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    /// <summary>
    /// Jwt 配置项.
    /// </summary>
    public class JwtAppSettings
    {
        /// <summary>
        /// Gets or sets Secret.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets Audience.
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// Gets or sets Issuer.
        /// </summary>
        public string ValidIssuer { get; set; }

        /// <summary>
        /// Gets or sets 到期秒.
        /// </summary>
        public int ExpireSeconds { get; set; }

        /// <summary>
        /// Gets or sets Header Key.
        /// </summary>
        public string HeaderKey { get; set; }
    }
}