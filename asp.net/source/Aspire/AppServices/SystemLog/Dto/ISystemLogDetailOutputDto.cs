// <copyright file="ISystemLogDetailOutputDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.SystemLog
{
    /// <summary>
    /// System Log Detail Output Dto.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Id.</typeparam>
    public interface ISystemLogDetailOutputDto<TPrimaryKey> : ISystemLogFilterOutputDto<TPrimaryKey>
    {
    }
}
