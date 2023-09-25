using System;

namespace Mint.Core.Domain;
public record DomainEvent : IDomainEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime PublishedAt => DateTime.Now;
    public string UserId { get; set; }
}