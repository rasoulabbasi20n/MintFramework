using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mint.Core.Persistance;
using System.Data;

namespace Mint.Persistance.EntityFramework
{
    public class EntityFrameworkUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        protected TDbContext Context { get; set; }
        protected IDbContextTransaction? TransactionScope;

        public EntityFrameworkUnitOfWork(TDbContext context)
        {
            Context = context;
        }

        public Task<IDisposable> BeginTransactionScope(TransactionIsolationLevel transactionIsolationLevel, CancellationToken cancellationToken)
        {
            if (TransactionScope is not null)
            {
                throw new InvalidOperationException("a transaction is already begun.");
            }
            TransactionScope = Context.Database.BeginTransaction(Map(transactionIsolationLevel));
            return Task.FromResult(TransactionScope as IDisposable);
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
            await TransactionScope?.CommitAsync(cancellationToken);
            TransactionScope?.Dispose();
        }


        public async Task Rollback(CancellationToken cancellationToken)
        {
            await TransactionScope?.RollbackAsync(cancellationToken);
            TransactionScope?.Dispose();
        }

        private static IsolationLevel Map(TransactionIsolationLevel transactionIsolationLevel)
        {
            switch (transactionIsolationLevel)
            {
                case TransactionIsolationLevel.ReadCommitted:
                    return IsolationLevel.ReadCommitted;
                case TransactionIsolationLevel.ReadUncommitted:
                    return IsolationLevel.ReadUncommitted;
                case TransactionIsolationLevel.RepeatableRead:
                    return IsolationLevel.RepeatableRead;
                case TransactionIsolationLevel.Serializable:
                    return IsolationLevel.Serializable;
            }
            throw new ArgumentOutOfRangeException("transactionIsolationLevel");
        }

    }


    
}