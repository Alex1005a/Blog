using Blog.Entities.ViewModels;
using MediatR;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetViewArticleById : IRequest<ArticleViewModel>
    {
        public int Id { get; set; }
        public GetViewArticleById(int id)
        {
            Id = id;
        }
    }
}
