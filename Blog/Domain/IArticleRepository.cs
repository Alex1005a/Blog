using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public interface IArticleRepository
    {
        Task<int> Add(Article article);
        Task<int> CountByTitle(string searchTitle);
        Task DecrementRating(int id);
        Task<Article> GetById(int id);
        Task IncrementRating(int id);
        Task<List<Article>> SearchByTitle(int pageSize, int page, string searchTitle);
    }
}
