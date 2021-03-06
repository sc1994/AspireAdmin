using System.Threading.Tasks;

namespace Aspire.SystemLog
{
    /// <summary>
    /// 日志查询 应用服务
    /// </summary>
    abstract public class SystemLogAppService<
        TId,
        TFilterInputDto,
        TFilterOutputDto,
        TDetailOutputDto> : Application
        where TFilterInputDto : ISystemLogFilterInputDto
        where TFilterOutputDto : ISystemLogFilterOutputDto<TId>
        where TDetailOutputDto : ISystemLogDetailOutputDto<TId>
    {

        /// <summary>
        /// filter
        /// </summary>
        /// <param name="filterInput"></param>
        /// <returns></returns>
        abstract public Task<PagedResultDto<TFilterOutputDto>> FilterAsync(TFilterInputDto filterInput);

        /// <summary>
        /// get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        abstract public Task<TDetailOutputDto> GetDetailAsync(TId id);

        /// <summary>
        /// 可选择项
        /// </summary>
        /// <param name="filterInput"></param>
        /// <returns></returns>
        abstract public Task<SystemLogSelectItemsDto> GetSelectItems(TFilterInputDto filterInput);
    }
}
