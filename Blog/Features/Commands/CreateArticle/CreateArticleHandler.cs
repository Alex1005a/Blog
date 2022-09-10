using AutoMapper;
using Blog.Data;
using Blog.Entities.Models;
using MediatR;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticleHandler : IRequestHandler<CreateArticle, int>
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper _mapper;
        private readonly ElasticClient client;
        public CreateArticleHandler(ApplicationDbContext context, IMapper mapper, ElasticClient _client)
        {
            db = context;
            _mapper = mapper;
            client = _client;
        }
        public async Task<int> Handle(CreateArticle model, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(model);

            model.User.AddArticle(article);
            var addArticleTask = db.SaveChangesAsync();
            var indexArticleTask = client.IndexDocumentAsync(article);

            await Task.WhenAll(addArticleTask, indexArticleTask);

            return article.Id;
        }
    }
}
