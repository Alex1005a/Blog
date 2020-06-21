using Blog.Contracts.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IArticleRepository Article { get; }
        IVoteRepository Vote { get; }
        void Save();
    }
}
