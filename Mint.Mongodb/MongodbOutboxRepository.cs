using Mint.Core.Persistance;

namespace Mint.Mongodb
{
    public class MongodbOutboxRepository : IOutboxRepository
    {
        public Task Dispatch(OutboxMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task EnqueueMessage(OutboxMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OutboxMessage?> GetNextMessage(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
