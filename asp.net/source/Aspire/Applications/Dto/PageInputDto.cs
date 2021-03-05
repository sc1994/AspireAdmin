namespace Aspire
{
    /// <summary>
    /// 页输入
    /// </summary>
    public interface IPageInputDto
    {
        /// <summary>
        /// 页 索引
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 页 大小
        /// </summary>
        int PageSize { get; set; }
    }

    /// <inheritdoc />
    public class PageInputDto : IPageInputDto
    {
        /// <inheritdoc />
        public int PageIndex { get; set; } = 1;

        /// <inheritdoc />
        public int PageSize { get; set; } = 10;
    }
}
