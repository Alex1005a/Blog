using Blog.Contracts.Queryinterfaces;
using Blog.Data;
using Blog.Entities.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleByIdHandler : IQueryHandler<GetArticleById, Article>
    {
        private readonly ApplicationDbContext db;
        private readonly IDbConnectionFactory DbConnection;
        private readonly IDistributedCache _distributedCache;

        public GetArticleByIdHandler(ApplicationDbContext context, IDbConnectionFactory dbConnection, IDistributedCache distributedCache)
        {
            db = context;
            DbConnection = dbConnection;
            _distributedCache = distributedCache;
        }
        public async Task<Article> Execute(GetArticleById query)
        {
            using IDbConnection conn = DbConnection.GetDbConnection();

            string CacheKey = $"Article-{query.Id}";
            string CacheValue = await _distributedCache.GetStringAsync(CacheKey);

            var sql = @$"select * from Articles 
                         where Id = {query.Id}";

            Article Article;

            if (CacheValue != null)
            {
                Article = JsonConvert.DeserializeObject<Article>(CacheValue);
            }

            else
            {
                Article = (await conn.QueryAsync<Article>(sql)).First();
                await Task.Run(async () => await _distributedCache.AddCache(CacheKey, Article));
            }

            db.Entry(Article).State = EntityState.Unchanged;
            db.Entry(Article).Collection(u => u.Votes).Load();

            return Article;
        }
    }
}
