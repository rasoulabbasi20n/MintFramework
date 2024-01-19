namespace Mint.Core.Persistance
{
    public enum TransactionIsolationLevel
    {
        ReadCommitted,
        ReadUncommitted,
        RepeatableRead,
        Serializable,
    }
}
