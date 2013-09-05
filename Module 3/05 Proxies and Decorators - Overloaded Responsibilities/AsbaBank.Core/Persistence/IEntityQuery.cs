using System.Linq;

namespace AsbaBank.Core.Persistence
{
    public interface IEntityQuery
    {
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
        void EnableLazyLoading();
        void DisableLazyLoading();
    }
}