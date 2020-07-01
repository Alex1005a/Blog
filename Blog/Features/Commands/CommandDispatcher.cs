using Blog.Contracts;
using Blog.Contracts.CommandInterfeces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Blog.Features.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _resolver;
 
        public CommandDispatcher(IServiceProvider resolver)
        {
            _resolver = resolver;
        }
 
        public async Task<ICommonResult> Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null) throw new ArgumentNullException("command");
 
            var handler = _resolver.GetRequiredService<ICommandHandler<TCommand>>();
 
            if (handler == null) throw new ArgumentNullException("handler");

            return await handler.Execute(command);
        }
    }
}
