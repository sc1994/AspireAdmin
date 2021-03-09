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
        /// 用于在 http item 中 获取或设置 ICurrentUser 的 Key.
        /// </summary>
        public const string HttpItemsKey = "Aspire.CurrentUser";

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
