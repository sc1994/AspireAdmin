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
        private readonly LogItemsStore itemsStore;
        private readonly string node;
        private readonly string index;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogElasticSearchAppService"/> class.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="itemsStore">Items Store.</param>
        public SerilogElasticSearchAppService(
            IConfiguration config,
            LogItemsStore itemsStore)
        {
            this.itemsStore = itemsStore;
            this.node = config.GetConnectionString("ElasticSearch");
            this.index = config.GetConnectionString("ElasticSearchIndex");
        }

        private enum OperatorEnum
        {
            Term,
            Gte,
            Lte,
        }

        /// <inheritdoc />
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

            if (!string.IsNullOrWhiteSpace(filterInput.Title))
            {
                items.Add(GetQueryItem("fields.apiRouter.title", filterInput.Title, OperatorEnum.Term));
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

            using var client = new HttpClient { BaseAddress = new Uri(this.node) };
            var res = await client.PostAsJsonAsync(uri, dsl);
            var data = await res.Content
                .ReadAsStringAsync()
                .DeserializeObjectAsync<JObject>();
            return new PagedResultDto<SystemLogFilterOutputDto>(
                data["hits"]["hits"].Select(ToLogModel<SystemLogFilterOutputDto>),
                data["hits"]["total"]["value"].ToObject<int>());
        }

        /// <inheritdoc />
        public override async Task<SystemLogDetailOutputDto> GetDetailAsync(string id)
        {
            using var client = new HttpClient { BaseAddress = new Uri(this.node) };
            var data = await client.GetStringAsync(id).DeserializeObjectAsync<JObject>();
            return ToLogModel<SystemLogDetailOutputDto>(data);
        }

        /// <inheritdoc />
        public override async Task<SystemLogSelectItemsDto> GetSelectItems()
        {
            var items = LogItemsStore.GetItems().Select(x => x.DeserializeObject());
            var routers = items
                .Select(x => x["apiRouter"]?.ToString())
                .Select(x => x?.Trim('/'))
                .Where(x => !x.IsNullOrWhiteSpace())
                .OrderBy(x => x)
                .Distinct();
            return await Task.FromResult(new SystemLogSelectItemsDto
            {
                ApiMethods = items.Select(x => x["apiMethod"]?.ToString()).Where(x => !x.IsNullOrWhiteSpace()).Distinct().ToArray(),
                ServerAddress = items.Select(x => x["serverAddress"]?.ToString()).Where(x => !x.IsNullOrWhiteSpace()).Distinct().ToArray(),
                ApiRouters = GetRoutersTree(routers, 0).ToArray(),
                Titles = items.Select(x => x["title"]?.ToString()).Where(x => !x.IsNullOrWhiteSpace()).Distinct().ToArray(),
            });
        }

        /// <inheritdoc />
        public override async Task<bool> DeleteAllSelectItems()
        {
            this.itemsStore.ClearItemsStore();
            return await Task.FromResult(true);
        }

        private static string GetSplitItemByIndex(string str, int index)
        {
            return str.Split('/', StringSplitOptions.RemoveEmptyEntries).Skip(index).FirstOrDefault();
        }

        private static IEnumerable<TreeNodeDto> GetRoutersTree(IEnumerable<string> routers, int index)
        {
            var result = new List<TreeNodeDto>();

            foreach (var item in routers.GroupBy(x => GetSplitItemByIndex(x, index)))
            {
                if (item.Key.IsNullOrWhiteSpace())
                {
                    continue;
                }

                result.Add(new TreeNodeDto
                {
                    Label = item.Key,
                    Children = GetRoutersTree(item, index + 1),
                });
            }

            return result;
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
                Title = x["_source"]["fields"]["title"]?.ToString() ?? string.Empty,
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