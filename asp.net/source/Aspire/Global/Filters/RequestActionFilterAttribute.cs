// <copyright file="RequestActionFilterAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// 请求过滤器.
    /// </summary>
    public class RequestActionFilterAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var logWriter = ServiceLocator.ServiceProvider.GetService<ILogWriter>();
            logWriter.Information("Request Action Executing", context.ActionArguments);
            base.OnActionExecuting(context);
        }
    }
}
