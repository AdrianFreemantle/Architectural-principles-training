using System;
using System.Collections.Generic;
using System.Linq;

namespace EventDriven
{
    public abstract class Aggregate : EntityBase, IAggregate
    {
        private int version;
        private readonly HashSet<IDomainEvent> changes = new HashSet<IDomainEvent>();
        protected readonly HashSet<IEntity> Entities = new HashSet<IEntity>();

        protected Aggregate(IHaveIdentity identity) 
            : base(identity)
        {
        }

        IEnumerable<IDomainEvent> IAggregate.GetUncommittedEvents()
        {
            return changes.ToArray();
        }

        void IAggregate.ClearUncommittedEvents()
        {
            changes.Clear();
        }

        int IAggregate.GetVersion()
        {
            return version;
        }

        void IAggregate.LoadFromHistory(IEnumerable<IDomainEvent> domainEvents)
        {
            foreach (var @event in domainEvents)
            {
                ApplyEvent(@event);
                version = @event.Version;
            }
        }

        void IAggregate.RegisterOwnedEntity(IEntity entity)
        {
            RegisterOwnedEntity(entity);
        }

        protected virtual void RegisterOwnedEntity(IEntity entity)
        {
            if (entity.Identity.IsEmpty())
            {
                throw new InvalidOperationException("An entity must be assigned a non empty Identity");
            }

            if(Entities.Any(e => e.Equals(entity)))
            {
                throw new InvalidOperationException(String.Format("An entity of type {0} with identity {1} is already registered.", entity.GetType().Name, entity.Identity));
            }

            Entities.Add(entity);
        }

        protected TEntity GetEntity<TEntity>(IHaveIdentity entityId) where TEntity : IEntity
        {
            var entity = Entities.FirstOrDefault(e => e.Identity.Equals(entityId));

            if (entity == null)
            {
                throw new InvalidOperationException(String.Format("Entity with id of {0} could not be found on aggregate {1}", entityId, Identity));
            }

            return (TEntity)entity;
        }

        protected override void RaiseEvent(IDomainEvent @event)
        {
            SaveEvent(@event);
            ApplyEvent(@event);
        }

        protected override void SaveEvent(IDomainEvent @event)
        {
            version++;
            @event.Version = version;
            @event.Aggregate = Identity;
            changes.Add(@event);
        }

        protected override void ApplyEvent(IDomainEvent @event)
        {
            if (@event.Entity == null)
            {
                base.ApplyEvent(@event);
            }
            else
            {
                var entity = Entities.Single(e => e.Identity.Equals(@event.Entity));
                entity.ApplyEvent(@event);
            }
        }
    }
}