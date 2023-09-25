namespace Mint.Core.Application.Commands
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Task<CommandResult<TResult>> Execute(TCommand command, CancellationToken cancellationToken) ;
    }

    public interface ICommandHandler<TCommand>
    {
        Task<CommandResult> Execute(TCommand command, CancellationToken cancellationToken);
    }
}
