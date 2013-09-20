using System.Collections.Generic;

namespace Aggregates
{
    public interface IAggregate : IEntity
    {
        void RegisterOwnedEntity(IEntity entity);
    }
}