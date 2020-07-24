using Blog.Contracts.Queryinterfaces;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Blog.Extensions;
using Nest;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Blog.Entities.DTO;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticlesHandler : IQueryHandler<GetPageArticles, IndexViewModel>
    {
        private readonly IDbConnectionFactory db;
        private readonly IDistributedCache _distributedCache;
        private readonly ElasticClient client;

        public GetPageArticlesHandler(IDbConnectionFactory context, IDistributedCache distributedCache, ElasticClient _client)
        {
            db = context;
            _distributedCache = distributedCache;
            client = _client;
        }

        public async Task<IndexViewModel> Execute(GetPageArticles query)
        {
            //using var conn = db.GetDbConnection();

            int pageSize = 3;

            var result = client.Search<ArticleDTO>(descriptor => descriptor
                                .From((query.Page - 1) * pageSize)
                                .Size(pageSize)
                                .Query(q => q
                                   .Match(m => m
                                      .Field(f => f.Title)
                                      .Query(query.SearchString)
                                      .Fuzziness(Fuzziness.EditDistance(3))
                                   )
                                )
                           );

            //IEnumerable<ArticleDTO> source = result.Documents.Skip((query.Page - 1) * pageSize).Take(pageSize);
            //var items = source.Skip((query.Page - 1) * pageSize).Take(pageSize);

            int count;

            if (!string.IsNullOrEmpty(query.SearchString))
            {
                count = (int)client.Count<ArticleDTO>(descriptor => descriptor
                                .Query(q => q
                                   .Match(m => m
                                      .Field(f => f.Title)
                                      .Query(query.SearchString)
                                      .Fuzziness(Fuzziness.EditDistance(3))
                                   )
                                )
                           ).Count;
            }
            else
            {
                count = (int)(await client.CountAsync<ArticleDTO>()).Count;
            }

            PageViewModel pageViewModel = new PageViewModel(count, query.Page, pageSize);

            return new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Articles = result.Documents,
                SearchString = query.SearchString
            };
        }
    }
}
