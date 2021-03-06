// <copyright file="MapperProfileAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Mapper
{
    using System;

    /// <summary>
    /// 映射器描述.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapperProfileAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfileAttribute"/> class.
        /// </summary>
        /// <param name="type">映射类型.</param>
        /// <param name="isCanReverseMap">是否可以反转映射.</param>
        public MapperProfileAttribute(Type type, bool isCanReverseMap = true)
        {
            this.Type = type;
            this.IsCanReverseMap = isCanReverseMap;
        }

        /// <summary>
        /// Gets 映射类型.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets a value indicating whether 是否可以反转映射.
        /// </summary>
        public bool IsCanReverseMap { get; }
    }
}
