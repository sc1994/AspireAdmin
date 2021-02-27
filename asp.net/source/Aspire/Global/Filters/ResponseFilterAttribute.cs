using System.Diagnostics;

using Aspire.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspire
{
    /// <summary>
    /// 响应过滤器
    /// </summary>
    public class ResponseFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在控制器完成后检验控制器结果
        /// </summary>
        public override void OnActionExecuted(ActionExecutedContext cxt)
        {
            if (cxt.Exception != null) {
                if (cxt.Exception is FriendlyException friendlyException) {
                    cxt.Result = new OkObjectResult(new GlobalResponse {
                        Code = friendlyException.Code,
                        Result = friendlyException.Messages,
#if DEBUG
                        StackTrace = friendlyException.StackTrace.ToString()
#endif
                    });
                }
                else if (cxt.Exception is { } exception) {
                    cxt.Result = new OkObjectResult(new GlobalResponse {
                        Code = ResponseCode.InternalServerError.GetHashCode(),
                        Result = "内部服务异常",
#if DEBUG
                        StackTrace = exception.Demystify().ToString()
#endif
                    });
                }
                // TODO 日志
                cxt.Exception = null;
                base.OnActionExecuted(cxt);
                return;
            }
            else {
                // TODO 日志
                // EnhancedStackTrace.Current();
            }

            var result = (ObjectResult)cxt.Result;
            cxt.Result = new OkObjectResult(new {
                Code = ResponseCode.Ok.GetHashCode(),
                Data = result.Value
            });
            base.OnActionExecuted(cxt);
        }
    }
}
