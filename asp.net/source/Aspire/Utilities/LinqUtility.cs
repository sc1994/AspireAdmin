using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspire
{
    public static class LinqUtility
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array)
                action(item);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this Task<T[]> sourceAsync, Func<T, bool> predicate = null)
        {
            var source = await sourceAsync;
            return source.FirstOrDefault(predicate!);
        }

        public static async Task<T[]> ToArrayAsync<T>(this Task<List<T>> sourceAsync)
        {
            var source = await sourceAsync;
            return source.ToArray();
        }


    }
}
