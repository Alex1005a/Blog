using Blog.Contracts.Queryinterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Blog.Features.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _resolver;

        public QueryDispatcher(IServiceProvider resolver)
        {
            _resolver = resolver;
        }

        public Task<TResult> Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException("query");

            var handler = _resolver.GetRequiredService<IQueryHandler<TQuery, TResult>>();

            if (handler == null) throw new ArgumentNullException("handler");

            return handler.Execute(query);
        }
    }
}
