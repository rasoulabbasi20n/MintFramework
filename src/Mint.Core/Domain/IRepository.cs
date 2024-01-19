namespace Mint.Core.Domain;

public interface IRepository<T, in TKey> where T : IAggregateRoot<TKey>
{
}