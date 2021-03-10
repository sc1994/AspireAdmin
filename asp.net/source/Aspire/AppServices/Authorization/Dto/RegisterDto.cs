// <copyright file="RegisterDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authorization
{
    /// <summary>
    /// 注册.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Gets or sets 用户 身份唯一标识.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets 密码.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets 角色.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets 姓名.
        /// </summary>
        public string Name { get; set; }
    }
}