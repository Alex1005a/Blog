using Blog.Contracts.Queryinterfaces;
using Blog.Entities.Models;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetArticleById : IQuery<Article>
    {
        public int Id { get; set; }
        public GetArticleById(int id)
        {
            Id = id;
        }
    }
}
