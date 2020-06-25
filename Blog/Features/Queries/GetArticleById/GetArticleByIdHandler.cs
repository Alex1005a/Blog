using Blog.Contracts;
using Blog.Data;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleByIdHandler : IQueryHandler<GetArticleById, Article>
    {
        private readonly ApplicationDbContext db;
        public GetArticleByIdHandler(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<Article> Execute(GetArticleById query)
        {
            return await db.Articles.FindAsync(query.Id);
        }
    }
}
