namespace Entities
{
    public interface IMemento
    {
        IHaveIdentity Identity { get; set; }
    }
}