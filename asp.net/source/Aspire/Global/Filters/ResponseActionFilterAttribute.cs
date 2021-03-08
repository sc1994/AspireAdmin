// <copyright file="ResponseActionFilterAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// 响应过滤器.
    /// </summary>
    public class ResponseActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在控制器完成后检验控制器结果.
        /// </summary>
        /// <param name="context">Context.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                var result = (ObjectResult)context.Result;
                context.Result = new OkObjectResult(new GlobalResponse
                {
                    Code = ResponseCode.Ok.GetHashCode(),
                    Result = result.Value,
                });
            }
            else
            {
                // 鉴定异常类型
                if (context.Exception is FriendlyException friendlyException)
                {
                    context.Result = new OkObjectResult(new GlobalResponse
                    {
                        Code = friendlyException.Code,
                        Message = friendlyException.Messages,
#if DEBUG
                        StackTrace = friendlyException,
#endif
                    });
                }
                else if (context.Exception is { } exception)
                {
                    context.Result = new OkObjectResult(new GlobalResponse
                    {
                        Code = ResponseCode.InternalServerError.GetHashCode(),
                        Message = new[] { exception.Message },
#if DEBUG
                        StackTrace = exception,
#endif
                    });
                }

                context.Exception = null;
            }

            // TODO 日志
            base.OnActionExecuted(context);
        }
    }
}
