using Blog.Entities.Models;
using MediatR;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleById : IRequest<Article>
    {
        public int Id { get; set; }
        public GetArticleById(int id)
        {
            Id = id;
        }
    }
}
