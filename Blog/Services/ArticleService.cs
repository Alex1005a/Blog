using AutoMapper;
using Blog.Contracts.Serviceinterfaces;
using Blog.Entities.ViewModels;
using Blog.Features.Commands.CreateArticle;
using Blog.Models;
using Ganss.XSS;
using Markdig;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ArticleService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<int> Create(CreateArticleViewModel model, User user)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Remove("form");

            string html = Markdown.ToHtml(model.Body, new MarkdownPipelineBuilder()
                                                          .UseAdvancedExtensions()
                                                          .Build());
            model.Body = sanitizer.Sanitize(html);

            var createArticle = _mapper.Map<CreateArticle>(model);
            createArticle.User = user;
            
            return await _mediator.Send(createArticle);
        }
    }
}
