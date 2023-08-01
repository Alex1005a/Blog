using System.Threading.Tasks;

namespace Blog.Domain
{
    public interface IVoteRepository
    {
        Task Add(Vote vote);
        Task<Vote> GetById(string userId, int articleId);
    }
}
