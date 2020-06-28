using AutoMapper;
using Blog.Contracts;
using Blog.Contracts.Queryinterfaces;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleByIdHandler : IQueryHandler<GetArticleById, ArticleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDbConnectionFactory db;
        public GetArticleByIdHandler(IDbConnectionFactory context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModel> Execute(GetArticleById query)
        {
            using IDbConnection conn = db.GetDbConnection();
            var sql = @$"select * from Articles a
                         JOIN AspNetUsers u
                         ON a.UserId = u.Id
                         where a.Id = {query.Id}";
            var Articles = await conn.QueryAsync<Article, User, ArticleViewModel>
                (sql, (Article, User) => {
                    var articleView = _mapper.Map<ArticleViewModel>(Article);
                    articleView.User = User;
                    return articleView;
                });
            Articles.First().Votes = conn.Query<Vote>(@$"SELECT * FROM Votes WHERE ArticleId = {query.Id}").ToList();

            return Articles.First();
            //return _mapper.Map<ArticleViewModel>(await db.Articles.FindAsync(query.Id)); 
        }
    }
}
