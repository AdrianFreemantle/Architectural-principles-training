using System;
using System.Collections.Generic;
using System.Linq;

namespace Aggregates
{
    public abstract class Aggregate : EntityBase, IAggregate
    {
        protected readonly HashSet<IEntity> Entities = new HashSet<IEntity>();

        protected Aggregate(IHaveIdentity identity) 
            : base(identity)
        {
        }

        internal virtual void RegisterOwnedEntity(IEntity entity)
        {
            if (entity.Identity.IsEmpty())
            {
                throw new InvalidOperationException("An entity must be assigned a non empty Identity");
            }

            if(Entities.Any(e => e.Equals(entity)))
            {
                throw new InvalidOperationException(String.Format("An entity of type {0} with identity {1} is already registered on aggregate {2}", entity.GetType().Name, entity.Identity, Identity));
            }

            Entities.Add(entity);
        }

        protected TEntity Get<TEntity>(IHaveIdentity entityId) where TEntity : IEntity
        {
            var entity = Entities.SingleOrDefault(e => e.Identity.Equals(entityId));

            if (entity == null)
            {
                throw new InvalidOperationException(String.Format("Entity {0} could not be found on aggregate {1}", entityId, Identity));
            }

            return (TEntity)entity;
        }

        protected TEntity RestoreEntity<TEntity>(IMemento memento) where TEntity : class, IEntity
        {
            IEntity entity = ActivatorHelper.CreateInstanceUsingNonPublicConstructor<TEntity>(this, memento.Identity);
            entity.RestoreSnapshot(memento);
            return (TEntity)entity;
        }
    }
}