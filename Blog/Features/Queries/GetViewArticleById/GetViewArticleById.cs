using Blog.Contracts;
using Blog.Contracts.Queryinterfaces;
using Blog.Entities.ViewModels;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetViewArticleById : IQuery<ArticleViewModel>
    {
        public int Id { get; set; }
        public GetViewArticleById(int id)
        {
            Id = id;
        }
    }
}
