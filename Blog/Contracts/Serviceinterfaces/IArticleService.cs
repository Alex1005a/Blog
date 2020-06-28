using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Contracts.Serviceinterfaces
{
    public interface IArticleService
    {
        Task<int> Create(CreateArticleViewModel model, User user);
        Task<ArticleViewModel> GetArticleById(int Id);
        Task<IndexViewModel> GetArticles(int page);
    }
}
