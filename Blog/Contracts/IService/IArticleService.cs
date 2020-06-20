using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts.IService
{
    public interface IArticleService
    {
        Task Create(CreateArticleViewModel model, string userId);
        Task<Article> GetArticle(int Id);
    }
}
