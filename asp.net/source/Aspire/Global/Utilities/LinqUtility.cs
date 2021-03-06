using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspire
{
    /// <summary>
    /// Linq 工具
    /// </summary>
    public static class LinqUtility
    {
        /// <summary>
        /// 迭代 Array 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array)
                action(item);
        }

        /// <summary>
        /// 迭代 IEnumerable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (var item in array)
                action(item);
        }

        /// <summary>
        /// 第一个或者默认
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceAsync"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        async public static Task<T> FirstOrDefaultAsync<T>(this Task<T[]> sourceAsync, Func<T, bool> predicate = null)
        {
            var source = await sourceAsync;
            return source.FirstOrDefault(predicate ?? (x => true));
        }

        /// <summary>
        /// 第一个或者默认
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceAsync"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        async public static Task<T> FirstOrDefaultAsync<T>(this Task<List<T>> sourceAsync, Func<T, bool> predicate = null)
        {
            var source = await sourceAsync;
            return source.FirstOrDefault(predicate ?? (x => true));
        }

        /// <summary>
        /// To Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceAsync"></param>
        /// <returns></returns>
        async public static Task<T[]> ToArrayAsync<T>(this Task<List<T>> sourceAsync)
        {
            var source = await sourceAsync;
            return source.ToArray();
        }

        /// <summary>
        /// Join 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
    }
}
