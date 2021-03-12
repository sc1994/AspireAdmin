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
            if (context.Result is ObjectResult objectResult)
            {
                OkObjectResult result;
                if (context.Exception is null)
                {
                    context.Result = result = new OkObjectResult(new GlobalResponse
                    {
                        Code = ResponseCode.Ok.GetHashCode(),
                        Result = objectResult.Value,
                    });
                }
                else
                {
                    switch (context.Exception)
                    {
                        // 鉴定异常类型
                        case FriendlyException friendlyException:
                            context.Result = result = new OkObjectResult(new GlobalResponse
                            {
                                Code = friendlyException.Code,
                                Message = friendlyException.Messages,
#if DEBUG
                                StackTrace = friendlyException,
#endif
                            });
                            break;
                        default:
                            context.Result = result = new OkObjectResult(new GlobalResponse
                            {
                                Code = ResponseCode.InternalServerError.GetHashCode(),
                                Message = new[] { context.Exception.Message },
#if DEBUG
                                StackTrace = context.Exception,
#endif
                            });
                            break;
                    }

                    context.Exception = null;
                }

                var logWriter = ServiceLocator.ServiceProvider.GetService<ILogWriter>();
                logWriter.Information("Response Action Executed", result.Value);
            }

            base.OnActionExecuted(context);
        }
    }
}
