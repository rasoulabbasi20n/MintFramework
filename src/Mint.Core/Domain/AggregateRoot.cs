using System;
using System.Collections.Concurrent;

namespace Mint.Core.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
{
    protected AggregateRoot() { }
    protected AggregateRoot(TKey id) : base(id)
    {
    }
    
    protected IDomainEvent[] NoEvents=> Array.Empty<IDomainEvent>();

    protected IDomainEvent[] CreateEvents(params IDomainEvent[] events)
    {
        return events;  
    }
}

public abstract class AggregateRootWithGuid : AggregateRoot<Guid>
{
    protected AggregateRootWithGuid()
    {
        Id = Guid.NewGuid();
    }
}