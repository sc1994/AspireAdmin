using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace Aspire
{
    /// <summary>
    /// 应用程序 服务
    /// </summary>
    [DynamicWebApi]
    [Authorize]
    public abstract class AppService : IDynamicWebApi
    {

    }
}
