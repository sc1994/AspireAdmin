// <copyright file="CurrentUserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authorization
{
    /// <summary>
    /// 当前用户.
    /// </summary>
    public class CurrentUserDto
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets roles.
        /// </summary>
        public string[] Roles { get; set; }
    }
}