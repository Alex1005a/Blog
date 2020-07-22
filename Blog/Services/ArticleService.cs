using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Contracts.Queryinterfaces;
using Blog.Contracts.Serviceinterfaces;
using Blog.Entities.Models;
using Blog.Entities.ViewModels;
using Blog.Features.Commands.AddComment;
using Blog.Features.Commands.CreateArticle;
using Blog.Features.Queries.GetArticleById;
using Blog.Features.Queries.GetPageArticles;
using Blog.Models;
using Ganss.XSS;
using Markdig;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMapper _mapper;
        public ArticleService(ILogger<ArticleService> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IMapper mapper)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            _mapper = mapper;
        }

        public async Task AddComment(int articleId, string text, string userId)
        {
            var article = await _queryDispatcher.Execute<GetArticleById, Article>(new GetArticleById(articleId));
            var result = await Task.Run(() => _commandDispatcher.Execute(new AddComment(text, userId, article, DateTime.Now)));

            _logger.LogInformation($"User with Id {userId} add comment {result.Success}");
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
            
            var result = await Task.Run(() => _commandDispatcher.Execute(createArticle));

            _logger.LogInformation($"User with Id {user.Id} create new Article Id: {result.TotalResults}");

            return result.TotalResults.Value;
        }

        public async Task<ArticleViewModel> GetArticleById(int Id)
        {
            _logger.LogInformation($"Get Article By Id: {Id}");
            var articleById = new GetViewArticleById(Id);
            return await _queryDispatcher.Execute<GetViewArticleById, ArticleViewModel>(articleById);
        }

        public async Task<IndexViewModel> GetArticles(int page, string searchString)
        {
            return await _queryDispatcher.Execute<GetPageArticles, IndexViewModel>(new GetPageArticles(page, searchString));
        }
    }
}
