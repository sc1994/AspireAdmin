using System;
using System.Threading.Tasks;

using Aspire.Logger;

namespace Aspire.LogQuery
{
    /// <inheritdoc />
    public abstract class LogQueryAppService<TLogModel> : LogQueryAppService<TLogModel, Guid>
        where TLogModel : ILogModel
    {

    }

    /// <summary>
    /// 日志查询 应用服务
    /// </summary>
    public abstract class LogQueryAppService<TLogModel, TId> : Application
        where TLogModel : ILogModel<TId>
    {
        private readonly ILogReader<TLogModel, TId> _logReader;

        public LogQueryAppService()
        {
            _logReader = ServiceLocator.ServiceProvider.GetService<ILogReader<TLogModel, TId>>();
        }

        public virtual async Task<PagedResultDto<LogFindOutputDto>> FilterAsyer(LogQueryFilter filter)
        {
            var res = await _logReader.FilterAsync(filter);
            throw new NotImplementedException();
        }
    }
}
