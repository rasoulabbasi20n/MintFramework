namespace Mint.Core.Domain
{
    public interface IExecute<in TDomainCommand>
    {
        IDomainEvent[] Execute(TDomainCommand command);
    }
}
