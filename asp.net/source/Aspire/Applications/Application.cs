using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;

namespace Aspire
{
    /// <summary>
    /// 应用程序
    /// </summary>
    [DynamicWebApi]
    [Authorize]
    public abstract class Application : IDynamicWebApi
    {

    }
}
