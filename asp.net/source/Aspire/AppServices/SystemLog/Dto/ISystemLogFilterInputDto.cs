using System;

namespace Aspire.SystemLog
{
    /// <summary>
    /// 日志 查询过滤器
    /// </summary>
    public interface ISystemLogFilterInputDto : IPageInputDto, ISystemLogCommonDto
    {
        /// <summary>
        /// 创建时间 范围
        /// </summary>
        DateTime[] CreatedAtRange { get; set; }
    }
}