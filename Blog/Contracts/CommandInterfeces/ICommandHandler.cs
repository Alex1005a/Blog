using System.Threading.Tasks;

namespace Blog.Contracts.CommandInterfeces
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand  
    {  
        Task<CommonResult> Execute(TCommand model);
    }
}
