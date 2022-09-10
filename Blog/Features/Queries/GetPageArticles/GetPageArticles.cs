using Blog.Entities.ViewModels;
using MediatR;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticles : IRequest<IndexViewModel>
    {
        public int Page { get; set; }
        public string SearchString { get; set; }
        public GetPageArticles(int page, string searchString)
        {
            Page = page;
            SearchString = searchString;
        }
    }
}
