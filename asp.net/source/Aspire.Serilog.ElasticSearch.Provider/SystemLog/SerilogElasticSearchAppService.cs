
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Aspire.SystemLog;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

namespace Aspire.Serilog.ElasticSearch.Provider.SystemLog
{
    public class SerilogElasticSearchAppService : SystemLogAppService<
        string,
        SystemLogFilterInputDto,
        SystemLogFilterOutputDto,
        SystemLogDetailOutputDto>
    {
        private readonly string _node;
        private readonly string _index;
        private readonly ILogWriter _logWriter;

        public SerilogElasticSearchAppService(IConfiguration config, ILogWriter logWriter)
        {
            _node = config.GetConnectionString("ElasticSearch");
            _index = config.GetConnectionString("ElasticSearchIndex");
            _logWriter = logWriter;
        }

        private enum OperatorEnum
        {
            Term,
            Gte,
            Lte
        }

        private static object GetQueryItem(string field, object value, OperatorEnum operatorEnum)
        {
            return new {
                @bool = new {
                    filter = operatorEnum switch {
                        OperatorEnum.Term => new Dictionary<string, object> {
                            {
                                "term", new Dictionary<string, object> {
                                    {field, value}
                                }
                            }
                        },
                        OperatorEnum.Gte => new Dictionary<string, object> {
                            {
                                "range", new Dictionary<string, object> {
                                    {
                                        field, new {
                                            gte = value
                                        }
                                    }
                                }
                            }
                        },
                        OperatorEnum.Lte => new Dictionary<string, object> {
                            {
                                "range", new Dictionary<string, object> {
                                    {
                                        field, new {
                                            lte = value
                                        }
                                    }
                                }
                            }
                        },
                        _ => throw new ArgumentOutOfRangeException(nameof(operatorEnum), operatorEnum, null)
                    }
                }
            };
        }

        private static TOutput ToLogModel<TOutput>(JToken x)
            where TOutput : SystemLogFilterOutputDto, new()
        {
            return new TOutput {
                TraceId = x["_source"]["fields"]["traceId"]?.ToString() ?? string.Empty,
                ApiRouter = x["_source"]["fields"]["className"]?.ToString() ?? string.Empty,
                ApiMethod = x["_source"]["fields"]["className"]?.ToString() ?? string.Empty,
                Body = x["_source"]["fields"]["message"]?.ToString() ?? string.Empty,
                CreatedAt = x["_source"]["@timestamp"].ToObject<DateTime>(),
                Filter1 = x["_source"]["fields"]["f1"]?.ToString() ?? string.Empty,
                Filter2 = x["_source"]["fields"]["f2"]?.ToString() ?? string.Empty,
                Id = $"/{x["_index"]}/{x["_type"]}/{x["_id"]}",
                Level = x["_source"]["level"].ToString() switch {
                    "Information" => LogLevelEnum.Information,
                    "Error" => LogLevelEnum.Error,
                    "Warning" => LogLevelEnum.Warning,
                    _ => throw new NotSupportedException("not supported log level " + x["_source"]["level"])
                },
                ServerAddress = x["_source"]["fields"]["serverAddress"]?.ToString() ?? string.Empty,
                ClientAddress = x["_source"]["fields"]["clientAddress"]?.ToString() ?? string.Empty,
                TickForRequest = x["_source"]["fields"]["tickForRequest"]?.ToString()?.TryToDouble() ?? 0
            };
        }

        async override public Task<PagedResultDto<SystemLogFilterOutputDto>> FilterAsync(SystemLogFilterInputDto filterInput)
        {
            var items = new List<object>();
            if (!string.IsNullOrWhiteSpace(filterInput.ApiMethod)) {
                items.Add(GetQueryItem("fields.apiMethod.keyword", filterInput.ApiMethod, OperatorEnum.Term));
            }
            if (!string.IsNullOrWhiteSpace(filterInput.ApiRouter)) {
                items.Add(GetQueryItem("fields.apiRouter.keyword", filterInput.ApiRouter, OperatorEnum.Term));
            }
            if (!string.IsNullOrWhiteSpace(filterInput.Filter1)) {
                items.Add(GetQueryItem("fields.filter1.keyword", filterInput.Filter1, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.Filter2)) {
                items.Add(GetQueryItem("fields.filter2.keyword", filterInput.Filter2, OperatorEnum.Term));
            }

            if (filterInput.CreatedAtRange?.Length == 2) {
                items.Add(GetQueryItem("@timestamp", filterInput.CreatedAtRange[0], OperatorEnum.Gte));
                items.Add(GetQueryItem("@timestamp", filterInput.CreatedAtRange[1], OperatorEnum.Lte));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.TraceId)) {
                items.Add(GetQueryItem("fields.traceId", filterInput.TraceId, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.ClientAddress)) {
                items.Add(GetQueryItem("fields.clientAddress.keyword", filterInput.ClientAddress, OperatorEnum.Term));
            }

            if (!string.IsNullOrWhiteSpace(filterInput.ServerAddress)) {
                items.Add(GetQueryItem("fields.serverAddress.keyword", filterInput.ServerAddress, OperatorEnum.Term));
            }
            if (filterInput.Level is not null) {
                items.Add(GetQueryItem("level.keyword", filterInput.Level.ToString(), OperatorEnum.Term));
            }
            var dsl = new {
                from = (filterInput.PageIndex - 1) * filterInput.PageSize,
                size = filterInput.PageSize,
                query = new {
                    @bool = new {
                        filter = items
                    }
                },
            };
            var uri = $"/{_index}*/_search";
            _logWriter.Information(new {
                uri,
                dsl
            });
            using var client = new HttpClient { BaseAddress = new Uri(_node) };
            var res = await client.PostAsJsonAsync(uri, dsl);
            var data = await res.Content
                .ReadAsStringAsync()
                .DeserializeObjectAsync<JObject>();
            return new PagedResultDto<SystemLogFilterOutputDto>(
                data["hits"]["hits"].Select(ToLogModel<SystemLogFilterOutputDto>),
                data["hits"]["total"]["value"].ToObject<int>());
        }

        async override public Task<SystemLogDetailOutputDto> GetDetailAsync(string id)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_node) };
            var data = await client.GetStringAsync(id).DeserializeObjectAsync<JObject>();
            return ToLogModel<SystemLogDetailOutputDto>(data);
        }

        async override public Task<SystemLogSelectItemsDto> GetSelectItems(SystemLogFilterInputDto filterInput)
        {
            throw new NotImplementedException();
        }
    }
}