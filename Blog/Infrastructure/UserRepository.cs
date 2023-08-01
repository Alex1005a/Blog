using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User> GetById(string id)
        {
            return _context.Users.SingleAsync(user => user.Id == id);
        }
    }
}
