namespace EventDriven
{
    public class TypedEntity<TState> : Entity where TState : class, IMemento, new()
    {
        protected TState State;

        protected TypedEntity(IAggregate parent, IHaveIdentity identity)
            : base(parent, identity)
        {
            State = new TState();
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            State = (TState)memento;
        }

        protected override IMemento GetSnapshot()
        {
            return State;
        }

        protected override void GetEventHandlers()
        {
            //we no longer have to scan for event handlers as we directly apply events to our state
        }

        protected override void ApplyEvent(IDomainEvent @event)
        {
            ((dynamic)State).When((dynamic)@event);
        }
    }
}