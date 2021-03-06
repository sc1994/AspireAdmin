using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspire
{
    /// <summary>
    /// 响应过滤器
    /// </summary>
    public class ResponseActionFilterAttribute : ActionFilterAttribute
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
                        Message = friendlyException.Messages,
#if DEBUG
                        StackTrace = friendlyException
#endif
                    });
                }
                else if (cxt.Exception is { } exception) {
                    cxt.Result = new OkObjectResult(new GlobalResponse {
                        Code = ResponseCode.InternalServerError.GetHashCode(),
                        Message = new[] { exception.Message },
#if DEBUG
                        StackTrace = exception
#endif
                    });
                }
                cxt.Exception = null;
            }
            else {
                var result = (ObjectResult)cxt.Result;
                cxt.Result = new OkObjectResult(new GlobalResponse {
                    Code = ResponseCode.Ok.GetHashCode(),
                    Result = result.Value,
                });
            }

            // TODO 日志
            //JsonConvert.SerializeObject((OkObjectResult)cxt.Message);
            base.OnActionExecuted(cxt);
        }
    }
}
