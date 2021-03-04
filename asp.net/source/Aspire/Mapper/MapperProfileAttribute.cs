using System;

namespace Aspire.Mapper
{
    /// <summary>
    /// 映射器描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapperProfileAttribute : Attribute
    {
        /// <summary>
        /// 映射类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 是否可以反转映射
        /// </summary>
        public bool IsCanReverseMap { get; }

        /// <summary>
        /// 映射器描述
        /// </summary>
        /// <param name="type">映射类型</param>
        /// <param name="isCanReverseMap">是否可以反转映射</param>
        public MapperProfileAttribute(Type type, bool isCanReverseMap = true)
        {
            Type = type;
            IsCanReverseMap = isCanReverseMap;
        }
    }
}
