using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ElasticClient _searchClient;

        public ArticleRepository(ApplicationDbContext context, ElasticClient searchClient)
        {
            _context = context;
            _searchClient = searchClient;
        }

        public async Task<int> Add(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            await _searchClient.IndexDocumentAsync(article);
            return article.Id;
        }

        public async Task<int> CountByTitle(string searchTitle)
        {
            var countTask = await _searchClient.CountAsync<Article>(descriptor => descriptor
                                .Query(q => SearchQuery(q, searchTitle))
                              );
            return (int)countTask.Count;
        }

        public async Task DecrementRating(int id)
        {
            await ChangeRating(id, -1);
        }

        public async Task IncrementRating(int id)
        {
            await ChangeRating(id, 1);
        }

        public async Task<Article> GetById(int id)
        {
            var article = await _searchClient.SearchAsync<Article>(descriptor =>
                descriptor.Query(q =>
                        q.Ids(ids => ids
                            .Values(id))
                    ));
            return article.Documents.First();
        }

        public async Task<List<Article>> SearchByTitle(int pageSize, int page, string searchTitle)
        {
            var articles = await _searchClient.SearchAsync<Article>(descriptor => descriptor
                                .From((page - 1) * pageSize)
                                .Size(pageSize)
                                .Query(q => SearchQuery(q, searchTitle))
                             );
            return articles.Documents.ToList();
        }

        private static QueryContainer SearchQuery(QueryContainerDescriptor<Article> q, string searchTitle) => 
            q.Match(m => m.Field(f => f.Title)
                          .Query(searchTitle)
                          .Fuzziness(Fuzziness.EditDistance(3))
                       );

        private async Task ChangeRating(int id, int value)
        {
            await _searchClient.UpdateByQueryAsync<Article>(descriptor =>
                descriptor.Query(q =>
                        q.Ids(ids => ids
                            .Values(id))
                    ).Script(s => s
                    .Source("ctx._source.rating += params.counter")
                    .Params(p => p
                        .Add("counter", value)
                    ))
                );
            await _context.Articles.Where(article => article.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Rating, b => b.Rating + value));
        }
    }
}
