// <copyright file="IServiceProviderProxy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// di 服务提供 代理.
    /// </summary>
    public interface IServiceProviderProxy
    {
        /// <summary>
        /// Get Services.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>T.</returns>
        T GetService<T>();

        /// <summary>
        /// Get Services.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>T.</returns>
        IEnumerable<T> GetServices<T>();

        /// <summary>
        /// Get Services.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Type Instance.</returns>
        object GetService(Type type);

        /// <summary>
        /// Get Services.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Type Instance.</returns>
        IEnumerable<object> GetServices(Type type);
    }
}
