using System.Transactions;

namespace Mint.Core.Persistance
{
    public interface IUnitOfWork
    {
        Task<IDisposable> BeginTransactionScope(TransactionIsolationLevel transactionIsolationLevel, CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
        Task Rollback(CancellationToken cancellationToken);
    }
}
