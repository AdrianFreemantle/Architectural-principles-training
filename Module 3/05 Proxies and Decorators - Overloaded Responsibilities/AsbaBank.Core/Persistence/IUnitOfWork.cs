namespace AsbaBank.Core.Persistence
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
    }
}