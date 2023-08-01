using AutoMapper;
using Blog.Domain;
using Ganss.XSS;
using Markdig;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticleHandler : IRequestHandler<CreateArticle, int>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public CreateArticleHandler(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateArticle request, CancellationToken cancellationToken)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Remove("form");

            string html = Markdown.ToHtml(request.Body, new MarkdownPipelineBuilder()
                                                          .UseAdvancedExtensions()
                                                          .Build());
            request.Body = sanitizer.Sanitize(html);
            var article = _mapper.Map<Article>(request);
            await _articleRepository.Add(article);
            return article.Id;
        }
    }
}
