namespace Aspire.SystemLog
{
    /// <summary>
    /// 选项集合
    /// </summary>
    public class SystemLogSelectItemsDto
    {
        /// <summary>
        /// API路由
        /// </summary>
        public string[] ApiMethods { get; set; }
        /// <summary>
        /// API方法
        /// </summary>
        public string[] ApiRouters { get; set; }
        /// <summary>
        /// 过滤1
        /// </summary>
        public string[] Filter1s { get; set; }
        /// <summary>
        /// 过滤2
        /// </summary>
        public string[] Filter2S { get; set; }
        /// <summary>
        /// 过滤3
        /// </summary>
        public string[] Filter3s { get; set; }
        /// <summary>
        /// 过滤4
        /// </summary>
        public string[] Filter4s { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string[] ClientAddress { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string[] ServerAddress { get; set; }
    }
}
