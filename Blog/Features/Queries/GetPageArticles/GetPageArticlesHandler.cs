using Blog.Models.ViewModels;
using System.Threading.Tasks;
using Blog.Models.DTO;
using System.Threading;
using MediatR;
using Blog.Domain;
using System.Linq;
using AutoMapper;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticlesHandler : IRequestHandler<GetPageArticles, IndexViewModel>
    {
        private const int PageSize = 3;

        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;

        public GetPageArticlesHandler(IMapper mapper, IArticleRepository articleRepository)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
        }

        public async Task<IndexViewModel> Handle(GetPageArticles query, CancellationToken cancellationToken)
        {
            var articles = await _articleRepository.SearchByTitle(PageSize, query.Page, query.SearchString);
            var numberOfArticles = await _articleRepository.CountByTitle(query.SearchString);
           

            PageViewModel pageViewModel = new(numberOfArticles, query.Page, PageSize);

            return new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Articles = articles.Select(_mapper.Map<ArticleDTO>),
                SearchString = query.SearchString
            };
        }
    }
}
