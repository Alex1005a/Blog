using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Models;
using Nest;
using System.Threading.Tasks;

namespace Blog.Features.Commands.CreateArticle
{
    public class CreateArticleHandler : ICommandHandler<CreateArticle>
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
        public async Task<ICommonResult> Execute(CreateArticle model)
        {          
            var article = _mapper.Map<Article>(model);


            var addArticleTask = model.User.AddArticle(article, db);
            var indexArticleTask = client.IndexDocumentAsync(article);
            await Task.WhenAll(addArticleTask, indexArticleTask);

            return new CommonResult(article.Id, "article Add in DB!!!", true); 
        }
    }
}
