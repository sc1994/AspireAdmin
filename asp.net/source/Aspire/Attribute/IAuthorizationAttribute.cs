using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspire
{
    /// <summary>
    /// 授权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal class AuthorizationAttribute : System.Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items["User"] is ICurrentUser user) {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
