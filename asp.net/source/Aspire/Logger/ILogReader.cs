
using System;
using System.Threading.Tasks;

namespace Aspire.Logger
{
    /// <inheritdoc />
    public interface ILogReader<TLogModel> : ILogReader<TLogModel, Guid>
        where TLogModel : ILogModel
    {

    }

    /// <summary>
    /// 日志 读取
    /// </summary>
    public interface ILogReader<TLogModel, in TId>
        where TLogModel : ILogModel<TId>
    {
        /// <summary>
        /// 查找
        /// </summary>
        Task<PagedResultDto<TLogModel>> FilterAsync(LogQueryFilter queryFilter);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TLogModel> GetAsync(TId id);
    }
}
