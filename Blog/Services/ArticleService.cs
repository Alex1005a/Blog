using Blog.Contracts;
using Blog.Contracts.IService;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext db;

        public ArticleService(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task Create(CreateArticleViewModel model, string userId)
        {
            db.Articles.Add(new Article
            {
                Title = model.Title,
                Body = model.Body,
                UserId = userId
            });

            await db.SaveChangesAsync();
        }

        public async Task<Article> GetArticleById(int Id)
        {
            return await db.Articles.FindAsync(Id);
        }
    }
}
