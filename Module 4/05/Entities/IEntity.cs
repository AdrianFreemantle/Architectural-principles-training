namespace Entities
{
    public interface IEntity : IAmRestorable
    {
        IHaveIdentity Identity { get; }
        void ApplyEvent(IDomainEvent @event);
        void SaveEvent(IDomainEvent @event);
    }
}