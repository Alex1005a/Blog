using Blog.Contracts;
using Blog.Data;
using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticleHandler : ICommandHandler<CreateArticle>
    {
        private readonly ApplicationDbContext db;
        public CreateArticleHandler(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task Execute(CreateArticle model)
        {
            db.Articles.Add(new Article
            (
                model.Title,
                model.Body,
                model.UserId
            ));

            await db.SaveChangesAsync();
        }
    }
}
