using Blog.Contracts.Queryinterfaces;
using Blog.Entities.ViewModels;
using System.Threading.Tasks;
using Nest;
using Blog.Entities.DTO;
using System;

namespace Blog.Features.Queries.GetPageArticles
{
    public class GetPageArticlesHandler : IQueryHandler<GetPageArticles, IndexViewModel>
    {
        private readonly ElasticClient client;

        public GetPageArticlesHandler(ElasticClient _client)
        {
            client = _client;
        }

        public async Task<IndexViewModel> Execute(GetPageArticles query)
        {
            Func<QueryContainerDescriptor<ArticleDTO>, QueryContainer> searchQuery =
                q => q.Match(m => m
                               .Field(f => f.Title)
                               .Query(query.SearchString)
                                .Fuzziness(Fuzziness.EditDistance(3))
                             );

            int pageSize = 3;

            var resultTask = client.SearchAsync<ArticleDTO>(descriptor => descriptor
                                .From((query.Page - 1) * pageSize)
                                .Size(pageSize)
                                .Query(searchQuery)
                             );

            var countTask = client.CountAsync<ArticleDTO>(descriptor => descriptor
                                .Query(searchQuery)
                              );

            PageViewModel pageViewModel = new PageViewModel((int)(await countTask).Count, query.Page, pageSize);

            return new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Articles = (await resultTask).Documents,
                SearchString = query.SearchString
            };
        }
    }
}
