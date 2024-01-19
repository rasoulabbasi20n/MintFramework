using Mint.Core.Persistance;
using MongoDB.Driver;

namespace Mint.Mongodb
{
    public class MongodbContext
    {
        private string DbName { get; set; }
        public IMongoDatabase Database => Client.GetDatabase(DbName);
        private IMongoClient Client { get; set; }
        public IClientSessionHandle? ClientSessionHandle { get; set; }
        public bool IsSessionStarted => ClientSessionHandle is not null;

        public MongodbContext(string connectionString)
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

        public async Task<MongodbContext> StartSession(TransactionIsolationLevel transactionIsolationLevel, CancellationToken cancellationToken)
        {
            ClientSessionHandle = await Client.StartSessionAsync(cancellationToken: cancellationToken);
            ClientSessionHandle.StartTransaction(MapTransactionLevel(transactionIsolationLevel));
            return this;
        }

        private static TransactionOptions MapTransactionLevel(TransactionIsolationLevel transactionIsolationLevel)
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
