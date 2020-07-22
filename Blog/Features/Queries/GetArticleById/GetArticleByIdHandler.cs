using Blog.Contracts.Queryinterfaces;
using Blog.Data;
using Blog.Entities.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Blog.Extensions;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using System;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleByIdHandler : IQueryHandler<GetArticleById, Article>
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
        public async Task<Article> Execute(GetArticleById query)
        {
            using IDbConnection conn = DbConnection.GetDbConnection();

            string CacheKey = $"Article-{query.Id}";
            string CacheValue = await _distributedCache.GetStringAsync(CacheKey);

            var sql = @$"select * from Articles 
                         where Id = {query.Id}";

            Article Article;
            conn.Close();
            
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
