using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts.Queryinterfaces
{
    public interface IQueryDispatcher
    {
        Task<TResult> Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
