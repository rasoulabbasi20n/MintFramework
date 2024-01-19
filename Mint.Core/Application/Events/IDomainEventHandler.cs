using Mint.Core.Domain;

namespace Mint.Core.Application.Events
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        Task<IApplicationEvent[]> Handle(TEvent @event, CancellationToken cancellationToken = default);
    } 
}
