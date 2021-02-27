namespace Aspire
{
    /// <summary>
    /// StringUtility
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// 是 null 或 空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
