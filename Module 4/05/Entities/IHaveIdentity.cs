namespace Entities
{
    public interface IHaveIdentity
    {
        dynamic GetId();
        bool IsEmpty();
        string GetTag();
    }
}