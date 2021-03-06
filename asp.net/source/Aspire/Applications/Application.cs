// <copyright file="Application.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using Mapper;
    using Panda.DynamicWebApi;
    using Panda.DynamicWebApi.Attributes;

    /// <summary>
    /// 应用
    /// 作为控制器的最基层.
    /// </summary>
    [DynamicWebApi]
    [AuthorizeFilter]
    [ResponseActionFilter]
    public abstract class Application : IDynamicWebApi
    {
        /// <summary>
        /// Mapper.
        /// </summary>
        protected readonly IAspireMapper Mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        protected Application()
        {
            Mapper = ServiceLocator.ServiceProvider.GetService<IAspireMapper>();
        }

        /// <summary>
        /// 映射到.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        protected T MapTo<T>(object source)
        {
            return Mapper.MapTo<T>(source);
        }

        /// <summary>
        /// 映射到
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        protected TTarget MapTo<TSource, TTarget>(TSource source)
        {
            return Mapper.MapTo<TSource, TTarget>(source);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="messages">错误编码</param>
        protected static T Failure<T>(params string[] messages)
        {
            FriendlyThrowException.ThrowException(messages);
            return default;
        }

        /// <summary>
        /// 失败.
        /// </summary>
        /// <param name="code">错误编码.</param>
        /// <param name="messages">消息.</param>
        protected static T Failure<T>(ResponseCode code, params string[] messages)
        {
            FriendlyThrowException.ThrowException(code, messages);
            return default;
        }

        /// <summary>
        /// 失败.
        /// </summary>
        /// <param name="code">错误编码.</param>
        /// <param name="messages">消息.</param>
        protected static T Failure<T>(int code, params string[] messages)
        {
            FriendlyThrowException.ThrowException(code, messages);
            return default;
        }
    }
}
