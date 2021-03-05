using Aspire.SystemLog;

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    /// <summary>
    /// 系统日志 详情 输出
    /// </summary>
    public class SystemLogDetailOutputDto : SystemLogFilterOutputDto, ISystemLogDetailOutputDto<string>
    {
    }
}
