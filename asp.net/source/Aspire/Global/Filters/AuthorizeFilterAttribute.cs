// <copyright file="AuthorizeFilterAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Aspire.Authenticate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// 鉴权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        public AuthorizeFilterAttribute()
        {
            this.CurrentRoles = Array.Empty<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        /// <param name="roles">角色集合以,分割.</param>
        public AuthorizeFilterAttribute(string roles)
        {
            this.CurrentRoles = roles
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();
        }

        /// <summary>
        /// Gets or sets 角色.
        /// </summary>
        public string[] CurrentRoles { get; set; }

        /// <summary>
        /// 鉴权.
        /// </summary>
        /// <param name="context">Context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor contextActionDescriptor))
            {
                return;
            }

            var allowAnonymous = contextActionDescriptor
                .MethodInfo
                .GetCustomAttributes<AllowAnonymousAttribute>()
                .FirstOrDefault();
            if (allowAnonymous != null)
            {
                return;
            }

            // 尝试查找鉴权特性
            var authorize = contextActionDescriptor
                .MethodInfo
                .GetCustomAttributes<AuthorizeFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo
                .GetCustomAttributes<AuthorizeFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo.BaseType?
                .GetCustomAttributes<AuthorizeFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo.BaseType?.BaseType?
                .GetCustomAttributes<AuthorizeFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo.BaseType?.BaseType?.BaseType?
                .GetCustomAttributes<AuthorizeFilterAttribute>()
                .FirstOrDefault();

            if (authorize is null)
            {
                return;
            }

            // 类型错误
            if (!(context.HttpContext.Items["User"] is ICurrentUser user))
            {
                return;
            }

            // 用户不是admin
            if (user.Roles == Roles.Admin)
            {
                return;
            }

            var preResponse = new GlobalResponse
            {
                Message = new[] { $"接口需要指定[{authorize.CurrentRoles.Join(",")}]的角色权限" },
                Code = ResponseCode.UnauthorizedRoles.GetHashCode(),
            };
            // 配置了指定角色
            if (authorize.CurrentRoles.Any())
            {
                // 没有角色
                if (user.Roles.IsNullOrWhiteSpace())
                {
                    context.Result = new JsonResult(preResponse) { StatusCode = StatusCodes.Status403Forbidden };
                    return;
                }

                var useRoles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);

                // 角色不包含在指定角色中
                if (useRoles.All(x => !authorize.CurrentRoles.Contains(x)))
                {
                    context.Result = new JsonResult(preResponse) { StatusCode = StatusCodes.Status403Forbidden };
                }
            }
        }
    }
}
