// <copyright file="AspireMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.AutoMapper.Provider
{
    using Aspire.Mapper;
    using global::AutoMapper;

    /// <summary>
    /// Aspire Mapper.
    /// </summary>
    internal class AspireMapper : IAspireMapper
    {
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspireMapper"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper.IMapper.</param>
        public AspireMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public TTarget MapTo<TTarget>(object source)
        {
            return this.mapper.Map<TTarget>(source);
        }

        /// <inheritdoc/>
        public TTarget MapTo<TSource, TTarget>(TSource source)
        {
            return this.mapper.Map<TSource, TTarget>(source);
        }
    }
}
