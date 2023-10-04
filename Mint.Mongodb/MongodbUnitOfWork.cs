using Mint.Core.Persistance;
using MongoDB.Driver;

namespace Mint.Mongodb
{
    internal class MongodbUnitOfWork : IUnitOfWork
    {
        protected IMongoDatabase Database => Client.GetDatabase(DbName);
        protected IMongoClient Client { get; set; }
        private IClientSessionHandle? clientSessionHandle { get; set; }
        public string DbName { get; set; }

        public MongodbUnitOfWork(string connectionString)
        {
            var connectionParameters = connectionString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(i =>
                {
                    var temp = i.Split("=".ToCharArray());
                    return new { Key = temp[0], Vslue = temp[1] };
                }).ToDictionary(i => i.Key, i => i.Vslue, StringComparer.OrdinalIgnoreCase);

            var dataSource = connectionParameters["Data Source"];
            DbName = connectionParameters["DbName"];
            Client = new MongoClient(dataSource);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName = "")
        {
            return Database.GetCollection<TEntity>(string.IsNullOrEmpty(collectionName) ? typeof(TEntity).Name : collectionName);
        }

        public async Task<IDisposable> BeginTransactionScope(TransactionIsolationLevel transactionIsolationLevel, CancellationToken cancellationToken)
        {
            clientSessionHandle = await Client.StartSessionAsync(cancellationToken: cancellationToken);
            clientSessionHandle.StartTransaction(Map(transactionIsolationLevel));
            return clientSessionHandle;
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            if (clientSessionHandle is not null)
                await clientSessionHandle.CommitTransactionAsync(cancellationToken);
        }


        public async Task Rollback(CancellationToken cancellationToken)
        {
            if (clientSessionHandle is not null)
                await clientSessionHandle.AbortTransactionAsync(cancellationToken);
        }

        private TransactionOptions Map(TransactionIsolationLevel transactionIsolationLevel)
        {
            switch (transactionIsolationLevel)
            {
                case TransactionIsolationLevel.ReadCommitted:
                    return new TransactionOptions(ReadConcern.Majority);
                case TransactionIsolationLevel.ReadUncommitted:
                    return new TransactionOptions(ReadConcern.Available);
                case TransactionIsolationLevel.RepeatableRead:
                    return new TransactionOptions(ReadConcern.Snapshot);
                case TransactionIsolationLevel.Serializable:
                    return new TransactionOptions(ReadConcern.Linearizable);
                default:
                    break;
            }
            throw new ArgumentOutOfRangeException(nameof(transactionIsolationLevel));
        }
    }
}
