// <copyright file="IAspireMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Mapper
{
    /// <summary>
    /// Mapper.
    /// </summary>
    public interface IAspireMapper
    {
        /// <summary>
        /// 映射到.
        /// </summary>
        /// <typeparam name="TTarget">目标对象.</typeparam>
        /// <param name="source">源.</param>
        /// <returns>目标.</returns>
        TTarget MapTo<TTarget>(object source);

        /// <summary>
        /// 映射到.
        /// </summary>
        /// <typeparam name="TSource">源对象.</typeparam>
        /// <typeparam name="TTarget">目标对象.</typeparam>
        /// <param name="source">源.</param>
        /// <returns>目标.</returns>
        TTarget MapTo<TSource, TTarget>(TSource source);
    }
}
