namespace Aggregates
{
    public abstract class TypedAggregate<TAggregateState> : Aggregate where TAggregateState : class, new()
    {
        protected TAggregateState State;

        protected TypedAggregate(IHaveIdentity identity) 
            : base(identity)
        {
            State = new TAggregateState();
        }
    }
}