using Blog.Contracts;
using Blog.Contracts.Queryinterfaces;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticlesHandler : IQueryHandler<GetPageArticles, IndexViewModel>
    {
        private readonly IDbConnectionFactory db;
        public GetPageArticlesHandler(IDbConnectionFactory context)
        {
            db = context;
        }

        public async Task<IndexViewModel> Execute(GetPageArticles query)
        {
            int pageSize = 3;   // количество элементов на странице
            /*
            IQueryable<Article> source = db.Articles.Skip((query.Page - 1) * pageSize).Take(pageSize);
            var count = await source.CountAsync();
            var items = await source.Skip((query.Page - 1) * pageSize).Take(pageSize).ToListAsync();
            */
            var sql = @$"SELECT [a].[Id], [a].[Body], [a].[Title], [a].[UserId]
                         FROM [Articles] AS [a]
                         ORDER BY (SELECT 1)
                         OFFSET {(query.Page - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using var conn = db.GetDbConnection();
            var articles = await conn.QueryAsync<Article>(sql);
            int count = await conn.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM Articles");

            PageViewModel pageViewModel = new PageViewModel(count, query.Page, pageSize);
            return new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Articles = articles
            };
        }
    }
}
