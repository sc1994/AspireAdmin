using System;
using System.Diagnostics.CodeAnalysis;
using Aspire.Logger;
using Aspire.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        [NotNull]
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

        /// <summary>
        /// Mapper 设置项
        /// <para>比如 new AutoMapperOptionsSetup()</para>
        /// </summary>
        [NotNull]
        public IAspireMapperOptionsSetup MapperOptions { get; set; }


        /// <summary>
        /// 审计仓储 设置项
        /// <para>比如 new FreeSqlAuditRepositoryOptionsSetup()</para>
        /// </summary>
        [NotNull]
        public IAuditRepositoryOptionsSetup AuditRepositoryOptions { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        [NotNull]
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// 日志 设置项
        /// </summary>
        [NotNull]
        public ILoggerOptionsSetup LoggerOptionsSetup { get; set; }
    }
}