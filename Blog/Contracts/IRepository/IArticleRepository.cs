using Blog.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts.IRepository
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
    }
}
