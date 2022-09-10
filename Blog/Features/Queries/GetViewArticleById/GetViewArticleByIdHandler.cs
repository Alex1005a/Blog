using AutoMapper;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Models;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Blog.Extensions;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Threading;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetViewArticleByIdHandler : IRequestHandler<GetViewArticleById, ArticleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory context;
        private readonly IDistributedCache _distributedCache;
        private readonly ApplicationDbContext db;

        public GetViewArticleByIdHandler(IDbConnectionFactory context, IMapper mapper, IDistributedCache distributedCache, ApplicationDbContext db)
        {
            this.context = context;
            _mapper = mapper;
            _distributedCache = distributedCache;
            this.db = db;
        }

        public async Task<ArticleViewModel> Handle(GetViewArticleById query, CancellationToken cancellationToken)
        {
            string CacheKey = $"ViewArticle-{query.Id}";
            var getStringTask = _distributedCache.GetStringAsync(CacheKey);

            using IDbConnection conn = context.GetDbConnection();

            ArticleViewModel article;
            string CacheValue = await getStringTask;
            if (CacheValue != null)
            {
                article = JsonConvert.DeserializeObject<ArticleViewModel>(CacheValue);
            }

            else
            {               
                var sql = @$"select * from Articles a
                             JOIN AspNetUsers u
                             ON a.UserId = u.Id
                             where a.Id = {query.Id}";

                article = (await conn.QueryAsync<Article, User, ArticleViewModel>
                    (sql, (article, user) => {
                        var articleView = _mapper.Map<ArticleViewModel>(article);
                        articleView.User = user;
                        return articleView;
                    })
                    ).First();

                await _distributedCache.AddCache(CacheKey, article);
            }

            var getVotesTask = conn.QueryAsync<Vote>(@$"SELECT * FROM Votes WHERE ArticleId = {query.Id}");
            var getCommentsTask = conn.QueryAsync<Comment>(@$"SELECT * FROM Comments WHERE ArticleId = {query.Id}");

            article.Votes = (await getVotesTask).ToList();
            var Comments = (await getCommentsTask).ToList(); ;
            foreach(var a in Comments)
            {
                db.Entry(a).State = EntityState.Unchanged;
                db.Entry(a).Reference(u => u.User).Load();
            }
            article.Comments = Comments;
            conn.Close();
            return article;
        }
    }
}
