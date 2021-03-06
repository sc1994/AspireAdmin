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
        public static string SerializeObject(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// 序列化异步任务的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        async public static Task<string> SerializeObjectAsync<T>(this Task<T> source)
        {
            var tmp = await source;
            return JsonConvert.SerializeObject(tmp);
        }

        /// <summary>
        /// 序列化 对象
        /// </summary>
        public static T DeserializeObject<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }

        /// <summary>
        /// 序列化异步任务的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        async public static Task<T> DeserializeObjectAsync<T>(this Task<string> source)
        {
            var tmp = await source;
            return JsonConvert.DeserializeObject<T>(tmp);
        }

        /// <summary>
        /// try to double
        /// </summary>
        /// <param name="source"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static double TryToDouble(this string source, double def = 0D)
        {
            if (source is not null && double.TryParse(source, out var val)) {
                return val;
            }
            return def;
        }
    }
}
