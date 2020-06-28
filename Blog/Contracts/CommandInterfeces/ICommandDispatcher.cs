using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts.CommandInterfeces
{
    public interface ICommandDispatcher
    {
        Task<CommonResult> Execute<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
