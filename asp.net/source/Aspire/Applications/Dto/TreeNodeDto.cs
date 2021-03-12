// <copyright file="TreeNodeDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Tree Node Dto.
    /// </summary>
    public class TreeNodeDto
    {
        /// <summary>
        /// Gets or sets Children.
        /// </summary>
        public IEnumerable<TreeNodeDto> Children { get; set; }

        /// <summary>
        /// Gets or sets Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }
}
