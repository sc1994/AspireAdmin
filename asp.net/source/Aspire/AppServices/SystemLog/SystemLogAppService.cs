// <copyright file="SystemLogAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.SystemLog
{
    using System.Threading.Tasks;

    /// <summary>
    /// System Log.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Primary Key.</typeparam>
    /// <typeparam name="TFilterInputDto">Filter Input Dto.</typeparam>
    /// <typeparam name="TFilterOutputDto">Filter Output Dto.</typeparam>
    /// <typeparam name="TDetailOutputDto">Detail Output Dto.</typeparam>
    public abstract class SystemLogAppService<
        TPrimaryKey,
        TFilterInputDto,
        TFilterOutputDto,
        TDetailOutputDto> : Application
        where TFilterInputDto : ISystemLogFilterInputDto
        where TFilterOutputDto : ISystemLogFilterOutputDto<TPrimaryKey>
        where TDetailOutputDto : ISystemLogDetailOutputDto<TPrimaryKey>
    {
        /// <summary>
        /// filter.
        /// </summary>
        /// <param name="filterInput">Filter Input.</param>
        /// <returns>分页过滤输出.</returns>
        public abstract Task<PagedResultDto<TFilterOutputDto>> FilterAsync(TFilterInputDto filterInput);

        /// <summary>
        /// Get Detail.
        /// </summary>
        /// <param name="id">Primary Key.</param>
        /// <returns>详情输出.</returns>
        public abstract Task<TDetailOutputDto> GetDetailAsync(TPrimaryKey id);

        /// <summary>
        /// 获取选择项.
        /// </summary>
        /// <returns>选择项集合.</returns>
        public abstract Task<SystemLogSelectItemsDto> GetSelectItems();

        /// <summary>
        /// 删除全部选择项.
        /// </summary>
        /// <returns>Is Success.</returns>
        public abstract Task<bool> DeleteAllSelectItems();
    }
}
