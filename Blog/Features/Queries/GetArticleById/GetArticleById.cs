using Blog.Contracts;
using Blog.Entities.ViewModels;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleById : IQuery<ArticleViewModel>
    {
        public int Id { get; set; }
        public GetArticleById(int id)
        {
            Id = id;
        }
    }
}
