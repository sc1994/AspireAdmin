// <copyright file="ConversionTypeUtility.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// 转换 类型.
    /// </summary>
    public static class ConversionTypeUtility
    {
        /// <summary>
        /// 序列化 对象.
        /// </summary>
        /// <param name="source">Source Object.</param>
        /// <returns>Json.</returns>
        public static string SerializeObject(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// 序列化异步任务的结果.
        /// </summary>
        /// <typeparam name="T">Object Type.</typeparam>
        /// <param name="source">Source Object.</param>
        /// <returns>Json.</returns>
        public static async Task<string> SerializeObjectAsync<T>(this Task<T> source)
        {
            var tmp = await source;
            return JsonConvert.SerializeObject(tmp);
        }

        /// <summary>
        /// 序列化 对象.
        /// </summary>
        /// <param name="source">Source Json.</param>
        /// <returns>Object.</returns>
        public static JObject DeserializeObject(this string source)
        {
            return JsonConvert.DeserializeObject<JObject>(source);
        }

        /// <summary>
        /// 序列化 对象.
        /// </summary>
        /// <typeparam name="T">Object Type.</typeparam>
        /// <param name="source">Source Json.</param>
        /// <returns>Object.</returns>
        public static T DeserializeObject<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }

        /// <summary>
        /// 序列化异步任务的结果.
        /// </summary>
        /// <param name="source">Source Json.</param>
        /// <returns>Object.</returns>
        public static async Task<JObject> DeserializeObjectAsync(this Task<string> source)
        {
            var tmp = await source;
            return JsonConvert.DeserializeObject<JObject>(tmp);
        }

        /// <summary>
        /// 序列化异步任务的结果.
        /// </summary>
        /// <typeparam name="T">Object Type.</typeparam>
        /// <param name="source">Source Json.</param>
        /// <returns>Object.</returns>
        public static async Task<T> DeserializeObjectAsync<T>(this Task<string> source)
        {
            var tmp = await source;
            return JsonConvert.DeserializeObject<T>(tmp);
        }

        /// <summary>
        /// Try To Double.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="def">Default.</param>
        /// <returns>Double.</returns>
        public static double TryToDouble(this string source, double def = 0D)
        {
            if (source is not null && double.TryParse(source, out var val))
            {
                return val;
            }

            return def;
        }
    }
}
