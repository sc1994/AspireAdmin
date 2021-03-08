// <copyright file="SerilogElasticSearchAppService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Aspire.Cache;
    using Aspire.SystemLog;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// SerilogElastic Search AppService.
    /// </summary>
    public class SerilogElasticSearchAppService : SystemLogAppService<
        string,
        SystemLogFilterInputDto,
        SystemLogFilterOutputDto,
        SystemLogDetailOutputDto>
    {
        private readonly string node;
        private readonly string index;
        private readonly ILogWriter logWriter;
        private readonly IAspireRedis redis;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogElasticSearchAppService"/> class.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="logWriter">Log Writer.</param>
        /// <param name="redis">Redis.</param>
        public SerilogElasticSearchAppService(IConfiguration config, ILogWriter logWriter, IAspireRedis redis)
        {
            this.node = config.GetConnectionString("ElasticSearch");
            this.index = config.GetConnectionString("ElasticSearchIndex");
            this.logWriter = logWriter;
            this.redis = redis;
        }

        private enum OperatorEnum
        {
            Term,
            Gte,
            Lte,
        }

        /// <summary>
        /// Filter.
        /// </summary>
        /// <param name="filterInput">Filter Input.</param>
        /// <returns>Page Output.</returns>
        public override async Task<PagedResultDto<SystemLogFilterOutputDto>> FilterAsync(SystemLogFilterInputDto filterInput)
        {
            var items = new List<object>();
            if (!string.IsNullOrWhiteSpace(filterInput.ApiMethod))
            {
                items.Add(GetQueryItem("fields.apiMethod.keyword", filterInput.ApiMethod, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.ApiRouter))
            {
                items.Add(GetQueryItem("fields.apiRouter.keyword", filterInput.ApiRouter, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.Filter1))
            {
                items.Add(GetQueryItem("fields.filter1.keyword", filterInput.Filter1, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.Filter2))
            {
                items.Add(GetQueryItem("fields.filter2.keyword", filterInput.Filter2, OperatorEnum.Term));
            }

            if (filterInput.CreatedAtRange?.Length == 2)
            {
                items.Add(GetQueryItem("@timestamp", filterInput.CreatedAtRange[0], OperatorEnum.Gte));
                items.Add(GetQueryItem("@timestamp", filterInput.CreatedAtRange[1], OperatorEnum.Lte));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.TraceId))
            {
                items.Add(GetQueryItem("fields.traceId", filterInput.TraceId, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.ClientAddress))
            {
                items.Add(GetQueryItem("fields.clientAddress.keyword", filterInput.ClientAddress, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.ServerAddress))
            {
                items.Add(GetQueryItem("fields.serverAddress.keyword", filterInput.ServerAddress, OperatorEnum.Term));
            }

            if (filterInput.Level is not null)
            {
                items.Add(GetQueryItem("level.keyword", filterInput.Level.ToString(), OperatorEnum.Term));
            }

            var dsl = new
            {
                from = (filterInput.PageIndex - 1) * filterInput.PageSize,
                size = filterInput.PageSize,
                query = new
                {
                    @bool = new
                    {
                        filter = items,
                    },
                },
            };
            var uri = $"/{this.index}*/_search";
            this.logWriter.Information(new
            {
                uri,
                dsl,
            });
            using var client = new HttpClient { BaseAddress = new Uri(this.node) };
            var res = await client.PostAsJsonAsync(uri, dsl);
            var data = await res.Content
                .ReadAsStringAsync()
                .DeserializeObjectAsync<JObject>();
            return new PagedResultDto<SystemLogFilterOutputDto>(
                data["hits"]["hits"].Select(ToLogModel<SystemLogFilterOutputDto>),
                data["hits"]["total"]["value"].ToObject<int>());
        }

        /// <summary>
        /// Get Detail.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Output.</returns>
        public override async Task<SystemLogDetailOutputDto> GetDetailAsync(string id)
        {
            using var client = new HttpClient { BaseAddress = new Uri(this.node) };
            var data = await client.GetStringAsync(id).DeserializeObjectAsync<JObject>();
            return ToLogModel<SystemLogDetailOutputDto>(data);
        }

        /// <summary>
        /// Get Select Items.
        /// </summary>
        /// <param name="filterInput">Filter Input.</param>
        /// <returns>Items.</returns>
        public override async Task<SystemLogSelectItemsDto> GetSelectItems(SystemLogFilterInputDto filterInput)
        {
            return await Task.FromResult(new SystemLogSelectItemsDto
            {
                ApiMethods = this.redis.GetSetAllMembers(LogWriter.RedisKeyApiMethod).ToArray(),
                ServerAddress = this.redis.GetSetAllMembers(LogWriter.RedisKeyServerAddress).ToArray(),
                ApiRouters = this.redis.GetSetAllMembers(LogWriter.RedisKeyApiRouter).ToArray(),
            });
        }

        private static object GetQueryItem(string field, object value, OperatorEnum operatorEnum)
        {
            return new
            {
                @bool = new
                {
                    filter = operatorEnum switch
                    {
                        OperatorEnum.Term => new
                        {
                            term = new Dictionary<string, object>
                            {
                                { field, value },
                            },
                        },
                        OperatorEnum.Gte => new
                        {
                            range = new Dictionary<string, object>
                            {
                                { field, new { gte = value, } },
                            },
                        },
                        OperatorEnum.Lte => (object)new
                        {
                            range = new Dictionary<string, object>
                            {
                                { field, new { lte = value, } },
                            },
                        },
                        _ => throw new ArgumentOutOfRangeException(nameof(operatorEnum), operatorEnum, null),
                    },
                },
            };
        }

        private static TOutput ToLogModel<TOutput>(JToken x)
            where TOutput : SystemLogFilterOutputDto, new()
        {
            return new TOutput
            {
                TraceId = x["_source"]["fields"]["traceId"]?.ToString() ?? string.Empty,
                ApiRouter = x["_source"]["fields"]["className"]?.ToString() ?? string.Empty,
                ApiMethod = x["_source"]["fields"]["className"]?.ToString() ?? string.Empty,
                Message = x["_source"]["fields"]["message"]?.ToString() ?? string.Empty,
                CreatedAt = x["_source"]["@timestamp"].ToObject<DateTime>(),
                Filter1 = x["_source"]["fields"]["f1"]?.ToString() ?? string.Empty,
                Filter2 = x["_source"]["fields"]["f2"]?.ToString() ?? string.Empty,
                Id = $"/{x["_index"]}/{x["_type"]}/{x["_id"]}",
                Level = x["_source"]["level"].ToString() switch
                {
                    "Information" => LogLevelEnum.Information,
                    "Error" => LogLevelEnum.Error,
                    "Warning" => LogLevelEnum.Warning,
                    _ => throw new NotSupportedException("not supported log level " + x["_source"]["level"])
                },
                ServerAddress = x["_source"]["fields"]["serverAddress"]?.ToString() ?? string.Empty,
                ClientAddress = x["_source"]["fields"]["clientAddress"]?.ToString() ?? string.Empty,
                TickForRequest = x["_source"]["fields"]["tickForRequest"]?.ToString()?.TryToDouble() ?? 0,
            };
        }
    }
}