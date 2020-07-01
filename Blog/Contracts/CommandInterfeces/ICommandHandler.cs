using System.Threading.Tasks;

namespace Blog.Contracts.CommandInterfeces
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand  
    {  
        Task<ICommonResult> Execute(TCommand model);
    }
}
