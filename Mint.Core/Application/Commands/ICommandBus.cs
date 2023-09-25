namespace Mint.Core.Application.Commands
{
    public interface ICommandBus
    {
        Task<ClientResponse> Send<TCommand>(TCommand command, CommandOptions commandOptions = default, CancellationToken cancellationToken= default);
        Task<ClientResponse<TResult>> SendAndReply<TCommand, TResult>(TCommand command, CommandOptions commandOptions = default, CancellationToken cancellationToken = default);
    }
}
