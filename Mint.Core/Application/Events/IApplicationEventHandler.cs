namespace Mint.Core.Application.Events
{
    public interface IApplicationEventHandler<in TEvent> where TEvent : IApplicationEvent
    {
        Task<IApplicationEvent[]> Handle(TEvent @event, CancellationToken cancellationToken = default);
    }
}
