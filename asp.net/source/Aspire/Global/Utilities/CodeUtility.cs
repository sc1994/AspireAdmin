namespace Aspire
{
    using System;
    using System.Text;

    /// <summary>
    /// 编码工具
    /// </summary>
    public static class CodeUtility
    {
        /// <summary>
        /// 编码到Base64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string EncodingToBase64(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
