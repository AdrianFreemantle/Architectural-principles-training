namespace Aggregates
{
    public class TypedEntity<TState> : Entity where TState : class, IMemento, new()
    {
        protected TState State;

        protected TypedEntity(IHaveIdentity identity) 
            : base(identity)
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
    }
}