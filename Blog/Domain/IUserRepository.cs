using System.Threading.Tasks;

namespace Blog.Domain
{
    public interface IUserRepository
    {
        Task<User> GetById(string id);
    }
}
