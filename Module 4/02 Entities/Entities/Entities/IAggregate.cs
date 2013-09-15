using System.Collections.Generic;

namespace Entities
{
    public interface IAggregate : IEntity
    {
        IEnumerable<IDomainEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
        int GetVersion();
        void LoadFromHistory(IEnumerable<IDomainEvent> domainEvents);
        void RegisterOwnedEntity(IEntity entity);
    }
}