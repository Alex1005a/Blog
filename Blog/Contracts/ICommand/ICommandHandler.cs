using Blog.Data;
using System.Threading.Tasks;

namespace Blog.Contracts
{
    public interface ICommandHandler<in TCommand> where TCommand:ICommand  
    {  
        Task Execute(TCommand model);
    }
}
