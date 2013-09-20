namespace EventDriven
{
    public abstract class TypedAggregate<TAggregateState> : Aggregate where TAggregateState : class, new()
    {
        protected TAggregateState State;

        protected TypedAggregate(IHaveIdentity identity) 
            : base(identity)
        {
            State = new TAggregateState();
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