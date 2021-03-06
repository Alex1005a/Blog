﻿using AutoMapper;
using Blog.Contracts.Queryinterfaces;
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

namespace Blog.Features.Queries.GetArticleById
{
    public class GetViewArticleByIdHandler : IQueryHandler<GetViewArticleById, ArticleViewModel>
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

        public async Task<ArticleViewModel> Execute(GetViewArticleById query)
        {
            using IDbConnection conn = context.GetDbConnection();

            string CacheKey = $"ViewArticle-{query.Id}";
            string CacheValue = await _distributedCache.GetStringAsync(CacheKey);

            ArticleViewModel article;

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

                await Task.Run(async() => await _distributedCache.AddCache(CacheKey, article));
            }

            article.Votes = conn.Query<Vote>(@$"SELECT * FROM Votes WHERE ArticleId = {query.Id}").ToList();
            var Comments = conn.Query<Comment>(@$"SELECT * FROM Comments WHERE ArticleId = {query.Id}").ToList();
            foreach(var a in Comments)
            {
                db.Entry(a).State = EntityState.Unchanged;
                db.Entry(a).Reference(u => u.User).Load();
            }
            article.Comments = Comments;
            conn.Close();
            return article;
            //return _mapper.Map<ArticleViewModel>(await db.Articles.FindAsync(query.Id)); 
        }
    }
}
