using Mint.Core.Application.Commands;
using Mint.Core.Application.Events;
using Mint.Core.Domain;
using Mint.Core.Persistance;
using Mint.Core.Problems;
using System.Text.Json;

namespace Mint.Core.Application
{
    public abstract class ServiceBusBase : IServiceBus
    {
        private readonly ILoggingService _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutboxRepository? outboxRepository;
        private readonly bool _outboxEnabled;

        protected ServiceBusBase(ILoggingService logger, IUnitOfWork unitOfWork, bool enableOutbox)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            if (enableOutbox)
            {
                outboxRepository = _unitOfWork.OutboxRepository;
                if (outboxRepository is null)
                {
                    throw new ArgumentNullException(nameof(outboxRepository), "unit of work must supply IOutboxRepository in order to enable outbox feature.");
                }
                _outboxEnabled = true;
            }
        }

        public async Task<ClientResponse> HandleCommand<TCommand>(TCommand command, CommandOptions? commandOptions = default, CancellationToken cancellationToken = default)
        {
            try
            {
                var handler = ResolveService<ICommandHandler<TCommand>>();
                var commandResult = await handler.Execute(command, cancellationToken);

                if (commandResult.Success)
                {
                    await CommitChanges(commandResult, cancellationToken);
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

        public async Task<ClientResponse<TResult>> HandleCommand<TCommand, TResult>(TCommand command, CommandOptions? commandOptions = default, CancellationToken cancellationToken = default)
        {
            try
            {
                var handler = ResolveService<ICommandHandler<TCommand, TResult>>();
                var commandResult = await handler.Execute(command, cancellationToken);

                if (commandResult.Success)
                {
                    await CommitChanges(commandResult, cancellationToken);
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

        protected abstract TService ResolveService<TService>();
        protected abstract IEnumerable<TService> ResolveServices<TService>();

        private async Task CommitChanges(CommandResult result, CancellationToken token)
        {
            var appEvents = new List<IApplicationEvent>(result.RaisedApplicationEvents);
            appEvents.AddRange(await ProcessDomainEvents(result.RaisedDomainEvents, token));
            await ProcessApplicationEvents(appEvents.ToArray(), token);
            await _unitOfWork.Commit(token);
        }

        private async Task<List<IApplicationEvent>> ProcessDomainEvents(IDomainEvent[] domainEvents, CancellationToken cancellationToken)
        {
            var appEvents = new List<IApplicationEvent>();
            foreach (var @event in domainEvents)
                appEvents.AddRange(await HandleDomainEvent(@event, cancellationToken));

            return appEvents;
        }

        private async Task<IApplicationEvent[]> HandleDomainEvent<T>(T @event, CancellationToken cancellationToken) where T : IDomainEvent
        {
            try
            {
                var handlers = ResolveServices<IDomainEventHandler<T>>();
                var appEvents = new List<IApplicationEvent>();
                foreach (var handler in handlers)
                    appEvents.AddRange(await handler.Handle(@event, cancellationToken));

                return appEvents.ToArray();
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.Error(ex);
                }
                catch
                {
                    // ignored
                }
                throw;
            }
        }

        private async Task ProcessApplicationEvents(IApplicationEvent[] applicationEvents, CancellationToken cancellationToken)
        {
            if (!_outboxEnabled && applicationEvents.Any())
                throw new InvalidOperationException("Can't dispatch application events with outbox being disabled. Application events are for creating new use case scenarios or new transactions (means that the origin transaction does not depend on the event handling result).");
           
            foreach (var item in applicationEvents)
            {
                await outboxRepository!.EnqueueMessage(new OutboxMessage { Data = JsonSerializer.Serialize(item) }, cancellationToken);
            }
        }

        private void HandleProblem(ProblemBase problem)
        {
            _logger.Error(problem);
        }
    }
}
