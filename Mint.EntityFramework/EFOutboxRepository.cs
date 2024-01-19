using Microsoft.EntityFrameworkCore;
using Mint.Core.Persistance;

namespace Mint.EntityFramework
{
    public class EFOutboxRepository : IOutboxRepository
    {
        protected DbSet<OutboxMessage> OutboxMessages { get; set; }

        public EFOutboxRepository(DbSet<OutboxMessage> outboxMessages)
        {
            OutboxMessages = outboxMessages;
        }

        public Task Dispatch(OutboxMessage message, CancellationToken cancellationToken)
        {
            message.Dispatch();
            OutboxMessages.Attach(message);
            return Task.CompletedTask;
        }

        public async Task EnqueueMessage(OutboxMessage message, CancellationToken cancellationToken)
        {
            await OutboxMessages.AddAsync(message, cancellationToken);
        }
 
        public OutboxMessage? GetNextMessage()
        {
            return OutboxMessages.OrderBy(x => x.PublishedDate).FirstOrDefault();
        }
    }
}
