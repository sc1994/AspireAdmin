using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc;

using Panda.DynamicWebApi;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Aspire
{
    /// <summary>
    /// aspire 启动 选项
    /// </summary>
    public class AspireSetupOptions
    {
        /// <summary>
        /// NewtonsoftJson 启动选项
        /// </summary>
        [AllowNull]
        public Action<MvcNewtonsoftJsonOptions> NewtonsoftJsonOptionsSetup { get; set; }

        /// <summary>
        /// 动态 api 启动选项
        /// <para>详情参考：https://github.com/pda-team/Panda.DynamicWebApi/blob/master/README_zh-CN.md</para>
        /// </summary>
        [NotNull]
        public Action<DynamicWebApiOptions> DynamicWebApiOptionsSetup { get; set; }

        /// <summary>
        /// swagger 启动选项
        /// </summary>
        [NotNull]
        public Action<SwaggerGenOptions> SwaggerGenOptionsSetup { get; set; }
    }
}