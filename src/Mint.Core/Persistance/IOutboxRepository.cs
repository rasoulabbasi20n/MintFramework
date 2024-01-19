namespace Mint.Core.Persistance
{
    public interface IOutboxRepository 
    {
        Task EnqueueMessage(OutboxMessage message, CancellationToken cancellationToken);
        OutboxMessage? GetNextMessage();
        Task Dispatch(OutboxMessage message, CancellationToken cancellationToken);
    }
}
