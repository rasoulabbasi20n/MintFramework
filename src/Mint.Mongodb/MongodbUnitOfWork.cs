using Mint.Core.Persistance;

namespace Mint.Mongodb
{
    public class MongodbUnitOfWork : IUnitOfWork
    {
        private MongodbContext Context { get; set; }

        public IOutboxRepository OutboxRepository => new MongodbOutboxRepository(Context);

        public MongodbUnitOfWork(string connectionString)
        {
            Context = new MongodbContext(connectionString);
        }

        public async Task<IDisposable> BeginTransactionScope(TransactionIsolationLevel transactionIsolationLevel, CancellationToken cancellationToken)
        {
            await Context.StartSession(transactionIsolationLevel, cancellationToken);
            return Context.ClientSessionHandle!;
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            if (Context.IsSessionStarted)
                await Context.ClientSessionHandle!.CommitTransactionAsync(cancellationToken);
        }

        public async Task Rollback(CancellationToken cancellationToken)
        {
            if (Context.IsSessionStarted)
                await Context.ClientSessionHandle!.AbortTransactionAsync(cancellationToken);
        }
    }
}
