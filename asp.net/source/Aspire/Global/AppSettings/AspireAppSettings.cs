// <copyright file="AspireAppSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    /// <summary>
    /// Aspire 配置项.
    /// </summary>
    public class AspireAppSettings
    {
        /// <summary>
        /// Gets or sets jWT.
        /// </summary>
        public JwtAppSettings Jwt { get; set; }

        /// <summary>
        /// Gets or sets 管理员.
        /// </summary>
        public AdminAppSettings Administrator { get; set; }
    }
}
