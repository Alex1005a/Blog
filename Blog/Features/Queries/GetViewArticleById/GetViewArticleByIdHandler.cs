using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using Blog.Domain;
using Blog.Models.ViewModels;

namespace Blog.Features.Queries.GetArticleById
{
    public class GetViewArticleByIdHandler : IRequestHandler<GetViewArticleById, ArticleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public GetViewArticleByIdHandler(
            IMapper mapper, 
            IDistributedCache distributedCache, 
            IArticleRepository articleRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _distributedCache = distributedCache;
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;

        }

        public async Task<ArticleViewModel> Handle(GetViewArticleById query, CancellationToken cancellationToken)
        {
            Article article;
            string cacheKey = $"ViewArticle-{query.Id}";
            var cacheArticle = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
            if (cacheArticle != null)
            {
                article = JsonConvert.DeserializeObject<Article>(cacheArticle);
            }
            else
            {
                article = await _articleRepository.GetById(query.Id);
                await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(article), cancellationToken);
            }
            ArticleViewModel articleView = _mapper.Map<ArticleViewModel>(article);
            articleView.Comments = await _commentRepository.GetByArticleId(query.Id);
            articleView.User = await _userRepository.GetById(article.UserId);
            return articleView;
        }
    }
}
