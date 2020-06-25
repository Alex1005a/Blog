using Blog.Contracts.IQuery;
using Blog.Contracts.IService;
using Blog.Data;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Features.Queries.GetArticleById;
using Castle.DynamicProxy.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<ArticleService> _logger;
        private readonly IQueryDispatcher _queryDispatcher;
        public ArticleService(ApplicationDbContext context, ILogger<ArticleService> logger, IQueryDispatcher queryDispatcher)
        {
            db = context;
            _logger = logger;
            _queryDispatcher = queryDispatcher;
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
            var articleById = new GetArticleById { Id = Id };
            return await _queryDispatcher.Execute<GetArticleById, Article>(articleById);
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            return await db.Articles.ToListAsync();
        }
    }
}
