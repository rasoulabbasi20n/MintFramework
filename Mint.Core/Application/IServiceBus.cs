using Mint.Core.Application.Commands;

namespace Mint.Core.Application
{
    public interface IServiceBus
    {
        void EnableOutbox();
        Task<ClientResponse> HandleCommand<TCommand>(TCommand command, CommandOptions? commandOptions = default, CancellationToken cancellationToken = default);
        Task<ClientResponse<TResult>> HandleCommand<TCommand, TResult>(TCommand command, CommandOptions? commandOptions = default, CancellationToken cancellationToken = default);
        Task Publish<TMessage>(TMessage message, CancellationToken token = default);
        Task Send<TMessage>(TMessage message, string destination, CancellationToken cancellationToken = default);
    }
}
