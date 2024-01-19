using Mint.Core.Application.Commands;

namespace Mint.Core.Application
{
    public interface IServiceBus
    {
        Task<ClientResponse> HandleCommand<TCommand>(TCommand command, CommandOptions? commandOptions = default, CancellationToken cancellationToken = default);
        Task<ClientResponse<TResult>> HandleCommand<TCommand, TResult>(TCommand command, CommandOptions? commandOptions = default, CancellationToken cancellationToken = default);
    }
}
