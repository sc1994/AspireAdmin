// <copyright file="LoginDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Authorization
{
    /// <summary>
    /// 登录 输入
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 用户 身份唯一标识
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}