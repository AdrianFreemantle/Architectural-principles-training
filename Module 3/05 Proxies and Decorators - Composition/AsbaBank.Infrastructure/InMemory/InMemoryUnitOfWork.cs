using System.Diagnostics;
using AsbaBank.Core;

namespace AsbaBank.Infrastructure.InMemory
{
    [DebuggerNonUserCode, DebuggerStepThrough]
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly JsonSerializer serializer = new JsonSerializer();
        private byte[] committedData;
        private readonly InMemoryDataStore dataStore = new InMemoryDataStore();
        
        public InMemoryUnitOfWork(InMemoryDataStore dataStore)
        {
            this.dataStore = dataStore;
            Commit();
        }

        public void Commit()
        {
            committedData = serializer.Serialize(dataStore);
        }

        public void Rollback()
        {
            var restored = serializer.Deserialize<InMemoryDataStore>(committedData);

            dataStore.Data = restored.Data;
            dataStore.EntityIdentities = restored.EntityIdentities;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where  TEntity : class
        {
            return new InMemoryRepository<TEntity>(dataStore);
        }
    }
}