using System;

namespace EventDriven
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredAt { get; }
        IHaveIdentity Entity { get; set; }
        IHaveIdentity Aggregate { get; set; }
        int Version { get; set; }
    }
}