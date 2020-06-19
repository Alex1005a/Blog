using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
