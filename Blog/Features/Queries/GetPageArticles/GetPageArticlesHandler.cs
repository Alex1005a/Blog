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
            using var conn = db.GetDbConnection();

            int pageSize = 3;

            if (!string.IsNullOrEmpty(query.SearchString))
            {
                var result = client.Search<ArticleDTO>(descriptor => descriptor
                                .Query(q => q
                                   .Match(m => m
                                      .Field(f => f.Title)
                                      .Query(query.SearchString)
                                      .Fuzziness(Fuzziness.EditDistance(3))
                                   )
                                )
                           );

                IEnumerable<ArticleDTO> source = result.Documents.Skip((query.Page - 1) * pageSize).Take(pageSize);
                var items = source.Skip((query.Page - 1) * pageSize).Take(pageSize);

                PageViewModel pageViewModel = new PageViewModel(result.Documents.Count, query.Page, pageSize);

                return new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    Articles = items,
                    SearchString = query.SearchString
                };
            }

            string CacheKey = $"PageArticles-{query.Page}";
            string CacheValue = await _distributedCache.GetStringAsync(CacheKey);

            if (CacheValue != null)
            {
                return JsonConvert.DeserializeObject<IndexViewModel>(CacheValue);
            }

            /*
            IQueryable<Article> source = db.Articles.Skip((query.Page - 1) * pageSize).Take(pageSize);
            var count = await source.CountAsync();
            var items = await source.Skip((query.Page - 1) * pageSize).Take(pageSize).ToListAsync();
            */

            var sql = @$"SELECT [a].[Id], [a].[Body], [a].[Title], [a].[UserId]
                         FROM [Articles] AS [a]
                         ORDER BY (SELECT 1)
                         OFFSET {(query.Page - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            

            var articles = await conn.QueryAsync<ArticleDTO>(sql);

            int count = await conn.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM Articles");

            PageViewModel pageViewModel1 = new PageViewModel(count, query.Page, pageSize);

            var model = new IndexViewModel
            {
                PageViewModel = pageViewModel1,
                Articles = articles,
                SearchString = query.SearchString
            };

            await Task.Run(async() => await _distributedCache.AddCache(CacheKey, model));

            return model;
        }
    }
}
