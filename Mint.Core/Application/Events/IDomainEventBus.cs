using Mint.Core.Domain;

namespace Mint.Core.Application.Events
{
    public interface IDomainEventBus
    {
        Task<IApplicationEvent[]> Publish<T>(T @event, CancellationToken cancellationToken = default) where T : IDomainEvent;
    }
}
