using Blog.Contracts.IRepository;
using Blog.Data;
using Blog.Entities.Models;

namespace Blog.Repository
{
    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }
}
