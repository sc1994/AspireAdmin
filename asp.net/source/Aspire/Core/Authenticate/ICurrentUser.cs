// <copyright file="ICurrentUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authenticate
{
    /// <summary>
    /// 当前登入用户.
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Gets or sets 用户Id.
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// Gets or sets 姓名.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets 角色.
        /// </summary>
        string Roles { get; set; }
    }
}
