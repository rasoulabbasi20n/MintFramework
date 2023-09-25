namespace Mint.Core.Application.Commands
{
    public record CommandOptions
    {
        public string TransactionIsolationLevel { get; set; }
    }
}
