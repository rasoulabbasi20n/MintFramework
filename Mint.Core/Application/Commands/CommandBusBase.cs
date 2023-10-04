using Mint.Core.Application.Events;
using Mint.Core.Domain;
using Mint.Core.Persistance;
using Mint.Core.Problems;

namespace Mint.Core.Application.Commands
{
    public abstract class CommandBusBase : ICommandBus
    {
        private readonly ILoggingService _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventBus _domainEventBus;

        protected CommandBusBase(ILoggingService logger, IUnitOfWork unitOfWork, IDomainEventBus domainEventBus)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _domainEventBus = domainEventBus;
        }

        public async Task<ClientResponse> Send<TCommand>(TCommand command, CommandOptions commandOptions = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var handler = ResolveHandler<TCommand>();
                var commandResult = await handler.Execute(command, cancellationToken);

                if (commandResult.Success)
                {
                    await CommitAndProcessEvents(commandResult, cancellationToken);
                    return ClientResponse.CreateSuccess();
                }

                HandleProblem(commandResult.Problem!);
                return ClientResponse.CreateFailure(commandResult.Problem!);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public async Task<ClientResponse<TResult>> SendAndReply<TCommand, TResult>(TCommand command, CommandOptions commandOptions = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var handler = ResolveHandler<TCommand, TResult>();
                var commandResult = await handler.Execute(command, cancellationToken);

                if (commandResult.Success)
                {
                    await CommitAndProcessEvents(commandResult, cancellationToken);
                    return ClientResponse<TResult>.CreateSuccess(commandResult.Result!);
                }

                HandleProblem(commandResult.Problem!);
                return ClientResponse<TResult>.CreateFailure(commandResult.Problem!);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        protected abstract ICommandHandler<TCommand> ResolveHandler<TCommand>();
        protected abstract ICommandHandler<TCommand, TResult> ResolveHandler<TCommand, TResult>();

        private async Task CommitAndProcessEvents(CommandResult result, CancellationToken token)
        {
            var appEvents = new List<IApplicationEvent>(result.RaisedApplicationEvents);
            appEvents.AddRange(await ProcessDomainEvents(result.RaisedDomainEvents, token));

            await _unitOfWork.Commit(token);

            await SendApplicationEvents(appEvents, token);
        }

        private async Task<List<IApplicationEvent>> ProcessDomainEvents(IDomainEvent[] domainEvents, CancellationToken cancellationToken)
        {
            var appEvents = new List<IApplicationEvent>();
            foreach (var @event in domainEvents)
                appEvents.AddRange(await _domainEventBus.Publish(@event, cancellationToken));

            return appEvents;
        }

        private async Task SendApplicationEvents(IEnumerable<IApplicationEvent> applicationEvents, CancellationToken cancellationToken)
        {
            // Method intentionally left empty.
        }

        private void HandleProblem(ProblemBase problem)
        {
            _logger.Error(problem);
        }
    }
}
