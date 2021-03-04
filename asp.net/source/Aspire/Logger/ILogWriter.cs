using System;
using System.Threading.Tasks;

namespace Aspire.Logger
{
    /// <summary>
    /// 日志 写入
    /// </summary>
    public interface ILogWriter<TCurrentClass>
    {
        /// <summary>
        /// write info
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        Task InfoAsync(string body, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write info
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public Task InfoAsync(object body, string filter1 = null, string filter2 = null)
        {
            return InfoAsync(body.ToJson(), filter1, filter2);
        }

        /// <summary>
        /// write warn
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        Task WarnAsync(string body, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write warn
        /// </summary>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public Task WarnAsync(object body, string filter1 = null, string filter2 = null)
        {
            return WarnAsync(body.ToJson(), filter1, filter2);
        }

        /// <summary>
        /// write error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        Task ErrorAsync(Exception ex, string body, string filter1 = null, string filter2 = null);

        /// <summary>
        /// write error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="body"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        public Task ErrorAsync(Exception ex, object body, string filter1 = null, string filter2 = null)
        {
            return ErrorAsync(ex, body.ToJson(), filter1, filter2);
        }
    }
}
