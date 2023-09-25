using System;

namespace Mint.Core.Domain;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime PublishedAt { get; }
    string UserId { get; set; }
}