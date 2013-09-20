using System;

namespace EventDriven
{
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; protected set; }
        public DateTime OccurredAt { get; protected set; }
        IHaveIdentity IDomainEvent.Entity { get; set; }
        IHaveIdentity IDomainEvent.Aggregate { get; set; }
        int IDomainEvent.Version { get; set; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
        }
    }
}