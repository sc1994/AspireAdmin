// <copyright file="IAspireMapperOptionsSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Mapper
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Aspire Mapper 启动.
    /// </summary>
    public interface IAspireMapperOptionsSetup
    {
        /// <summary>
        /// 添加 Aspire Mapper.
        /// <para>必须声明如何实现IAspireMapperStartup.</para>
        /// </summary>
        /// <param name="services">Service Collection.</param>
        void AddAspireMapper(IServiceCollection services);
    }
}
