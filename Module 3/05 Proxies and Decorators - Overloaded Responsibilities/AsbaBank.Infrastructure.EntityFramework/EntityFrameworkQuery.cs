using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AsbaBank.Core.Persistence;

namespace AsbaBank.Infrastructure.EntityFramework
{
    public class EntityFrameworkQuery : IEntityQuery, ISqlQuery, IDisposable
    {
        private readonly DbContext context;
        private bool disposed;

        public EntityFrameworkQuery(IContextFactory contextFactory)
        {
            context = contextFactory.GetContext();
            DisableLazyLoading();
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>().AsNoTracking();
        }

        public void EnableLazyLoading()
        {
            context.Configuration.LazyLoadingEnabled = true;
            context.Configuration.ProxyCreationEnabled = true;
        }

        public void DisableLazyLoading()
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            return context.Database.SqlQuery<TEntity>(sql, parameters);
        }

        public IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters)
        {
            return context.Database.SqlQuery(elementType, sql, parameters);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            context.Dispose();
        }
    }
}