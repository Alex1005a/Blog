using AutoMapper;
using Blog.Contracts.CommandInterfeces;
using Blog.Data;
using Blog.Entities.Models;
using Ganss.XSS;
using Markdig;
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

            await Task.Run(() =>
                model.User.AddArticle(article, db)
            );

            await client.IndexDocumentAsync(article);

            return new CommonResult(article.Id, "article Add in DB!!!", true); 
        }
    }
}
