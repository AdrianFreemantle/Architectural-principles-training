namespace Aggregates
{
    public interface IEntity : IAmRestorable
    {
        IHaveIdentity Identity { get; }
    }
}