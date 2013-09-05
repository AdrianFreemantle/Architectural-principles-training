using System;
using System.Data.Entity;
using AsbaBank.Core;
using AsbaBank.Infrastructure.Logging;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork, IDisposable
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(EntityFrameworkUnitOfWork));
        private readonly IContextFactory contextFactory;
        private DbContext context;
        private bool isDisposed;

        public EntityFrameworkUnitOfWork(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            context = contextFactory.GetContext();
        }

        public void Commit()
        {            
            context.SaveChanges();
            Logger.Verbose("Commited changes.");
        }

        public void Rollback()
        {
            context.Dispose();
            context = contextFactory.GetContext();
            Logger.Verbose("Rolled back all changes.");
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new EntityFrameworkRepository<TEntity>(context.Set<TEntity>());
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            context.Dispose();
        }
    }
}