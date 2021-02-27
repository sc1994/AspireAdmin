using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace Aspire
{
    /// <summary>
    /// 应用程序 服务
    /// </summary>
    [DynamicWebApi]
    [Authorization]
    public abstract class AppService : IDynamicWebApi
    {

    }
}
