// <copyright file="EnumUtility.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Enum Utility.
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// Get Description.
        /// </summary>
        /// <typeparam name="T">Some Enum.</typeparam>
        /// <param name="enumValue">Enum Value.</param>
        /// <returns>Description.</returns>
        public static string GetDescription<T>(this T enumValue)
            where T : Enum
        {
            return typeof(T)
                .GetField(enumValue.ToString())
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }
    }
}
