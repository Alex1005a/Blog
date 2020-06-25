using System.Threading.Tasks;

namespace Blog.Contracts
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Execute(TQuery query);
    }
}
