using Microsoft.Extensions.DependencyInjection;
using NexOrder.Framework.Core.Contracts;

namespace NexOrder.Framework.Core.Services
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : class
        {
            var handler = _serviceProvider.GetRequiredService<IHandler<TCommand, TResult>>();
            return await handler.Handle(command);
        }
    }
}
