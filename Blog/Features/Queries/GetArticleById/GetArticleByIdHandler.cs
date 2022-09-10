﻿using Blog.Data;
using Blog.Entities.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Blog.Extensions;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using MediatR;
using System.Threading;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleByIdHandler : IRequestHandler<GetArticleById, Article>
    {
        private readonly ApplicationDbContext db;
        private readonly IDbConnectionFactory DbConnection;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<GetArticleByIdHandler> log;

        public GetArticleByIdHandler(ApplicationDbContext context, IDbConnectionFactory dbConnection, IDistributedCache distributedCache, ILogger<GetArticleByIdHandler> _log)
        {
            db = context;
            DbConnection = dbConnection;
            _distributedCache = distributedCache;
            log = _log;
        }
        public async Task<Article> Handle(GetArticleById query, CancellationToken cancellationToken)
        {
            string CacheKey = $"Article-{query.Id}";
            var getStringTask = _distributedCache.GetStringAsync(CacheKey);

            using IDbConnection conn = DbConnection.GetDbConnection();

            var sql = @$"select * from Articles 
                         where Id = {query.Id}";

            Article Article;
            conn.Close();
            string CacheValue = await getStringTask;
            if (CacheValue != null)
            {
                try
                {
                    Article = JsonConvert.DeserializeObject<Article>(JsonConvert.DeserializeObject(CacheValue).ToString(), new JsonSerializerSettings { 
                        NullValueHandling = NullValueHandling.Ignore, 
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        ContractResolver = new PrivateResolver()
                    });
                }
                catch(Exception e)
                {
                    log.LogError(e.Message);
                    log.LogError(e.StackTrace);
                    log.LogError(e.Source);
                    log.LogError(e.InnerException.Message);
                    return null;
                }
            }

            else
            {
                Article = (await conn.QueryAsync<Article>(sql)).First();
                await Task.Run(async () => await _distributedCache.AddCache(CacheKey, JsonConvert.SerializeObject(Article)));
            }
            
            db.Entry(Article).State = EntityState.Unchanged;
            db.Entry(Article).Collection(u => u.Votes).Load();
            db.Entry(Article).Collection(u => u.Comments).Load();
            db.Entry(Article).Reference(u => u.User).Load();

            return Article;
        }
    }
}
