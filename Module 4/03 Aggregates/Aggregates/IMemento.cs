namespace Aggregates
{
    public interface IMemento
    {
        IHaveIdentity Identity { get; set; }
    }
}