using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public class VoteRepository : IVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public VoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public Task<Vote> GetById(string userId, int articleId)
        {
            return _context.Votes.SingleAsync(vote => vote.UserId == userId && vote.ArticleId == articleId);
        }
    }
}
