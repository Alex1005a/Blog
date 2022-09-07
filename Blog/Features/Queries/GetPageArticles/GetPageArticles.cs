using Blog.Contracts.Queryinterfaces;
using Blog.Entities.ViewModels;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticles : IQuery<IndexViewModel>
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
