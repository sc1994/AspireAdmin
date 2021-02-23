using System;

namespace Aspire
{
    /// <summary>
    /// GUID 工具
    /// </summary>
    public static class GuidUtility
    {
        /// <summary>
        /// 创建新的有序GUID
        /// </summary>
        /// <returns></returns>
        public static Guid NewOrderlyGuid()
        {
            return Guid.NewGuid(); // TODO 有序的GUID
        }
    }
}
