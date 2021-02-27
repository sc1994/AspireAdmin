using System;
using System.Linq;
using System.Reflection;

using Aspire.Authenticate;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspire
{
    /// <summary>
    /// 鉴权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string[] CurrentRoles { get; set; } = new string[0];

        /// <summary>
        /// 鉴权
        /// </summary>
        public AuthorizeAttribute()
        {

        }

        /// <summary>
        /// 鉴权 指定 角色
        /// </summary>
        public AuthorizeAttribute(string roles)
        {
            CurrentRoles = roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 鉴权
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor contextActionDescriptor) {
                var allowAnonymous = contextActionDescriptor
                    .MethodInfo
                    .GetCustomAttributes<AllowAnonymousAttribute>()
                    .FirstOrDefault();

                // 尝试查找鉴权特性
                var authorize = contextActionDescriptor
                    .MethodInfo
                    .GetCustomAttributes<AuthorizeAttribute>()
                    .FirstOrDefault() ?? contextActionDescriptor
                    .ControllerTypeInfo
                    .GetCustomAttributes<AuthorizeAttribute>()
                    .FirstOrDefault() ?? contextActionDescriptor
                    .ControllerTypeInfo.BaseType?
                    .GetCustomAttributes<AuthorizeAttribute>()
                    .FirstOrDefault() ?? contextActionDescriptor
                    .ControllerTypeInfo.BaseType?.BaseType?
                    .GetCustomAttributes<AuthorizeAttribute>()
                    .FirstOrDefault() ?? contextActionDescriptor
                    .ControllerTypeInfo.BaseType?.BaseType?.BaseType?
                    .GetCustomAttributes<AuthorizeAttribute>()
                    .FirstOrDefault();

                if (authorize is not null) {
                    // 用户不是admin
                    if (context.HttpContext.Items["User"] is ICurrentUser user && user.Roles != Roles.Admin) {
                        // 配置了指定角色
                        if (authorize.CurrentRoles.Any()) {
                            // 没有角色
                            if (user.Roles.IsNullOrWhiteSpace()) {
                                context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status403Forbidden };
                                return;
                            }
                            var useRoles = user.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            // 角色不包含在指定角色中
                            if (useRoles.All(x => !authorize.CurrentRoles.Contains(x))) {
                                context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status403Forbidden };
                            }
                        }
                    }
                    else if (allowAnonymous is null) {
                        context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
            }
        }
    }
}
