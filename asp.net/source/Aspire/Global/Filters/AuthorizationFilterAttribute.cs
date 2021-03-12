// <copyright file="AuthorizationFilterAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Aspire.Authenticate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationFilterAttribute"/> class.
        /// </summary>
        public AuthorizationFilterAttribute()
        {
            this.CurrentRoles = Array.Empty<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationFilterAttribute"/> class.
        /// </summary>
        /// <param name="roles">角色集合以,分割.</param>
        public AuthorizationFilterAttribute(string roles)
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
        [SuppressMessage("Style", "IDE0083:使用模式匹配", Justification = "<挂起>")]
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
                .GetCustomAttributes<AuthorizationFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo
                .GetCustomAttributes<AuthorizationFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo.BaseType?
                .GetCustomAttributes<AuthorizationFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo.BaseType?.BaseType?
                .GetCustomAttributes<AuthorizationFilterAttribute>()
                .FirstOrDefault() ?? contextActionDescriptor
                .ControllerTypeInfo.BaseType?.BaseType?.BaseType?
                .GetCustomAttributes<AuthorizationFilterAttribute>()
                .FirstOrDefault();

            if (authorize is null)
            {
                return;
            }

            // 类型错误
            if (!(context.HttpContext.Items[ICurrentUser.HttpItemsKey] is ICurrentUser user))
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
                Messages = new[] { $"接口需要指定[{authorize.CurrentRoles.Join(",")}]的角色权限" },
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
