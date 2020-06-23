using Blog.Contracts;
using Blog.Contracts.IService;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(ApplicationDbContext context, ILogger<ArticleService> logger)
        {
            db = context;
            _logger = logger;
        }

        public async Task Create(CreateArticleViewModel model, string userId)
        {
            db.Articles.Add(new Article
            (
                model.Title,
                model.Body,
                userId
            ));

            await db.SaveChangesAsync();

            _logger.LogInformation($"User with Id {userId} create new Article");
        }

        public async Task<Article> GetArticleById(int Id)
        {
            _logger.LogInformation($"Get Article By Id: {Id}");
            return await db.Articles.FindAsync(Id);
        }
    }
}
