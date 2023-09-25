using System.Collections.Concurrent;

namespace Mint.Core.Domain;

public interface IAggregateRoot
{
}

public interface IAggregateRoot<out TKey> : IAggregateRoot
{
    public TKey Id { get; }
}