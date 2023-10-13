using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mint.Core.Persistance;
using Mint.EntityFramework;
using System.Data;

namespace Mint.Persistance.EntityFramework
{
    public class EFUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        protected TDbContext Context { get; set; }

        public IOutboxRepository? OutboxRepository
        {
            get
            {
                try
                {
                    // TODO : check if it throws exception when trying to get the DBSET
                    return new EFOutboxRepository(Context.Set<OutboxMessage>());
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        protected IDbContextTransaction? TransactionScope;
        public EFUnitOfWork(TDbContext context)
        {
            Context = context;
        }

        public Task<IDisposable> BeginTransactionScope(TransactionIsolationLevel transactionIsolationLevel, CancellationToken cancellationToken)
        {
            if (TransactionScope is not null)
            {
                throw new InvalidOperationException("another transaction is already in use.");
            }
            TransactionScope = Context.Database.BeginTransaction(Map(transactionIsolationLevel));
            return Task.FromResult(TransactionScope as IDisposable);
        }

        public async Task Commit(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
            if (TransactionScope is not null)
            {
                await TransactionScope.CommitAsync(cancellationToken);
                TransactionScope.Dispose();
            }
        }


        public async Task Rollback(CancellationToken cancellationToken)
        {
            if (TransactionScope is not null)
            {
                await TransactionScope.RollbackAsync(cancellationToken);
                TransactionScope.Dispose();
            }
        }

        protected static IsolationLevel Map(TransactionIsolationLevel transactionIsolationLevel)
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

        //        public virtual IOutboxRepository? OutboxRepository
        //        {  
        //            try
        //            {
        //#pragma warning disable S1135 // Track uses of "TODO" tags
        //                // TODO : check if it throws exception when trying to get the DBSET
        //                return new EFOutboxRepository(Context.Set<OutboxMessage>());
        //#pragma warning restore S1135 // Track uses of "TODO" tags
        //            }
        //            catch IOutboxRepository? IUnitOfWork.OutboxRepository => throw new NotImplementedException();

        //        (Exception)
        //            {
        //                return null;
        //            }
        //}
        //        }
    }
}