namespace EventDriven
{
    public class Memento : IMemento
    {
        IHaveIdentity IMemento.Identity { get; set; }
    }
}