namespace Aspire.Dto
{
    /// <summary>
    /// 数据传输对象
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IDto<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public TPrimaryKey Id { get; set; }
    }
}
