namespace Mint.Core.Persistance
{
    public interface IOutboxRepository 
    {
        Task EnqueueMessage(OutboxMessage message, CancellationToken cancellationToken);
        Task<OutboxMessage?> GetNextMessage(CancellationToken cancellationToken);
        Task Dispatch(OutboxMessage message, CancellationToken cancellationToken);
    }
}
