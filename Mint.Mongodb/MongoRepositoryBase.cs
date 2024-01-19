using Mint.Core.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Mint.Mongodb
{
    public abstract class MongodbRepositoryBase<TEntity, TKey> where TEntity : AggregateRoot<TKey>
    {
        public MongodbContext DbContext { get; set; }
        protected abstract IMongoCollection<TEntity> GetCollection();
        protected MongodbRepositoryBase(MongodbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected Task InsertOne(TEntity entity,
            CancellationToken cancellationToken)
        {
            var collection = GetCollection();
            if (DbContext.IsSessionStarted)
            {
                return collection.InsertOneAsync(
                    DbContext.ClientSessionHandle,
                    entity,
                    cancellationToken: cancellationToken);
            }
            return collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        protected Task UpsertOne(TEntity entity,
            CancellationToken cancellationToken)
        {
            var collection = GetCollection();
            if (DbContext.IsSessionStarted)
            {
                return collection.ReplaceOneAsync(
                    DbContext.ClientSessionHandle,
                    Builders<TEntity>.Filter.Eq("_id", entity.Id),
                    entity,
                    new ReplaceOptions() { IsUpsert = true },
                    cancellationToken);
            }

            return collection.ReplaceOneAsync(
                     Builders<TEntity>.Filter.Eq("_id", entity.Id),
                     entity,
                     new ReplaceOptions() { IsUpsert = true },
                     cancellationToken);
        }

        protected IMongoQueryable<TEntity> AsQueryable()
        {
            var collection = GetCollection();
            return DbContext.IsSessionStarted ?
                collection.AsQueryable(DbContext.ClientSessionHandle) :
                collection.AsQueryable();
        }
    }
}
