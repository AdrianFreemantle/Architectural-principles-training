using System.Data.Entity;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public interface IContextFactory
    {
        DbContext GetContext();
    }
}