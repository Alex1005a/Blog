using Blog.Contracts;
using Blog.Contracts.Queryinterfaces;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticlesHandler : IQueryHandler<GetPageArticles, IndexViewModel>
    {
        private readonly IDbConnectionFactory db;
        private readonly IDistributedCache _distributedCache;
        public GetPageArticlesHandler(IDbConnectionFactory context, IDistributedCache distributedCache)
        {
            db = context;
            _distributedCache = distributedCache;
        }

        public async Task<IndexViewModel> Execute(GetPageArticles query)
        {
            using var conn = db.GetDbConnection();

            int pageSize = 3;

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

            

            var articles = await conn.QueryAsync<Article>(sql);

            int count = await conn.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM Articles");

            PageViewModel pageViewModel = new PageViewModel(count, query.Page, pageSize);

            var model = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Articles = articles
            };

            await Task.Run(async() => await _distributedCache.AddCache(CacheKey, model));

            return model;
        }
    }
}
