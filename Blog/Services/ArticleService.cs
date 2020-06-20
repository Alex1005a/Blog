using Blog.Contracts;
using Blog.Contracts.IService;
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
        private readonly IRepositoryWrapper _repoWrapper;

        public ArticleService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task Create(CreateArticleViewModel model, string userId)
        {
            _repoWrapper.Article.Create(new Article
            {
                Title = model.Title,
                Body = model.Body,
                UserId = userId
            });

            _repoWrapper.Save();
        }

        public async Task<Article> GetArticle(int Id)
        {
            return _repoWrapper.Article.FindByCondition(u => u.Id == Id).First();
        }
    }
}
