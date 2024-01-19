using Mint.Core.Persistance;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mint.Mongodb
{
    public class MongodbOutboxRepository : MongodbRepositoryBase<OutboxMessage, string>, IOutboxRepository
    {
        private readonly IMongoCollection<OutboxMessage> OutboxCollection;
        public MongodbOutboxRepository(MongodbContext context) : base(context)
        {
            OutboxCollection = DbContext.Database.GetCollection<OutboxMessage>("outbox", new MongoCollectionSettings { });
        }

        public async Task Dispatch(OutboxMessage message, CancellationToken cancellationToken)
        {
            message.Dispatch();
            await UpsertOne(message, cancellationToken);
        }

        public Task EnqueueMessage(OutboxMessage message, CancellationToken cancellationToken)
        {
            return InsertOne(message, cancellationToken);
        }

        public OutboxMessage? GetNextMessage()
        {
            return AsQueryable()
                .OrderBy(x => x.PublishedDate)
                .FirstOrDefault(x => !x.Dispatched);
        }

        protected override IMongoCollection<OutboxMessage> GetCollection()
        {
            return OutboxCollection;
        }
    }
}
