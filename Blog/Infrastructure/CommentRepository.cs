using Blog.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public Task<List<Comment>> GetByArticleId(int articleId)
        {
            return Task.FromResult(
                _context.Comments.Where(comment => comment.ArticleId == articleId).ToList()
                );
        }
    }
}
