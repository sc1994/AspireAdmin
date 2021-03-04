using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Aspire
{
    /// <summary>
    /// 转换 类型
    /// </summary>
    public static class ConversionTypeUtility
    {
        /// <summary>
        /// 序列化 对象
        /// </summary>
        public static string ToJson(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// 序列化异步任务的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async Task<string> ToJsonAsync<T>(this Task<T> source)
        {
            var tmp = await source;
            return JsonConvert.SerializeObject(tmp);
        }
    }
}
