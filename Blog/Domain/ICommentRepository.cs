using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);
        Task<List<Comment>> GetByArticleId(int articleId);
    }
}
