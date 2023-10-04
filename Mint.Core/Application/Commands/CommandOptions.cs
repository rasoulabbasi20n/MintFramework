namespace Mint.Core.Application.Commands
{
    public record CommandOptions
    {
        public TransactionIsolationLevel TransactionIsolationLevel { get; set; } = TransactionIsolationLevel.ReadCommitted;
    }
}
