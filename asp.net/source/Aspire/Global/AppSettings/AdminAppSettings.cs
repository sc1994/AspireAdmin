// <copyright file="AdminAppSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    /// <summary>
    /// 管理员 配置项.
    /// </summary>
    public class AdminAppSettings
    {
        /// <summary>
        /// Gets or sets 主键.
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets 用户名.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets 密码.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }
    }
}