// <copyright file="Application.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using Aspire.Mapper;

    using Panda.DynamicWebApi;
    using Panda.DynamicWebApi.Attributes;

    /// <summary>
    /// 应用
    /// 作为控制器的最基层.
    /// </summary>
    [DynamicWebApi]
    [AuthorizationFilter]
    [ResponseActionFilter]
    [RequestActionFilter]
    public abstract class Application : IDynamicWebApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        protected Application()
        {
            this.Mapper = ServiceLocator.ServiceProvider.GetService<IAspireMapper>();
        }

        /// <summary>
        /// Gets mapper.
        /// </summary>
        protected IAspireMapper Mapper { get; }

        /// <summary>
        /// 失败.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="messages">错误编码.</param>
        /// <returns>Return T.</returns>
        protected static T Failure<T>(params string[] messages)
        {
            FriendlyThrowException.ThrowException(messages);
            return default;
        }

        /// <summary>
        /// Failure.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="code">错误编码.</param>
        /// <param name="messages">消息.</param>
        /// <returns>Return T.</returns>
        protected static T Failure<T>(ResponseCode code, params string[] messages)
        {
            FriendlyThrowException.ThrowException(code, messages);
            return default;
        }

        /// <summary>
        /// Failure.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="code">错误编码.</param>
        /// <param name="title">Title.</param>
        /// <param name="messages">消息.</param>
        /// <returns>Return T.</returns>
        protected static T Failure<T>(int code, string title, params string[] messages)
        {
            FriendlyThrowException.ThrowException(code, title, messages);
            return default;
        }

        /// <summary>
        /// Map To.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="source">Source.</param>
        /// <returns>Return T.</returns>
        protected T MapTo<T>(object source)
        {
            return this.Mapper.MapTo<T>(source);
        }

        /// <summary>
        /// Map To.
        /// </summary>
        /// <typeparam name="TSource">Source.</typeparam>
        /// <typeparam name="TTarget">Target.</typeparam>
        /// <param name="source">Input Source.</param>
        /// <returns>Return Target.</returns>
        protected TTarget MapTo<TSource, TTarget>(TSource source)
        {
            return this.Mapper.MapTo<TSource, TTarget>(source);
        }
    }
}
