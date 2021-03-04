using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Aspire.Logger;

using Microsoft.Extensions.Configuration;

namespace Aspire.Serilog.ElasticSearch.Provider
{
    public class LogReader : ILogReader<LogModel>
    {
        private readonly string _node;
        private readonly string _index;

        public LogReader()
        {
            var config = ServiceLocator.ServiceProvider.GetService<IConfiguration>();
            _node = config.GetConnectionString("ElasticSearch");
            _index = config.GetConnectionString("ElasticSearchIndex");
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
                            { "term", new Dictionary<string, object> {
                                { field, value}
                            }}
                        },
                        OperatorEnum.Gte => new Dictionary<string, object> {
                            { "range", new Dictionary<string, object> {
                                { field, new {
                                    gte = value
                                }}
                            }
                        }},
                        OperatorEnum.Lte => new Dictionary<string, object> {
                            { "range", new Dictionary<string, object> {
                                { field, new {
                                    lte = value
                                }}
                            }
                        }},
                        _ => throw new ArgumentOutOfRangeException(nameof(operatorEnum), operatorEnum, null)
                    }
                }
            };
        }

        public async Task<PagedResultDto<LogModel>> FilterAsync(LogQueryFilter filter)
        {
            var items = new List<object>();
            if (!string.IsNullOrWhiteSpace(filter.ClassName)) {
                items.Add(GetQueryItem("fields.className", filter.ClassName, OperatorEnum.Term));
            }
            if (!string.IsNullOrWhiteSpace(filter.Filter1)) {
                items.Add(GetQueryItem("fields.filter1", filter.Filter1, OperatorEnum.Term));
            }
            if (!string.IsNullOrWhiteSpace(filter.Filter2)) {
                items.Add(GetQueryItem("fields.filter2", filter.Filter2, OperatorEnum.Term));
            }

            var dsl = new {
                from = 0,
                size = 10,
                query = new {
                    @bool = new {
                        filter = items
                    }
                },
            };
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_node);
            var res = await client.PostAsJsonAsync($"/{_index}*/_search", dsl);
            var data = await res.Content.ReadAsStringAsync();
            throw new NotImplementedException();
        }

        public async Task<LogModel> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
