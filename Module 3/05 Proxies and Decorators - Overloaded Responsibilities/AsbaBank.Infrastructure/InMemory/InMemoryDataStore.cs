using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace AsbaBank.Infrastructure.InMemory
{
    [DataContract, DebuggerNonUserCode, DebuggerStepThrough]
    public class InMemoryDataStore
    {
        [DataMember]
        internal Dictionary<string, object> Data { get; set; }
        [DataMember]
        internal Dictionary<string, int> EntityIdentities { get; set; }

        public InMemoryDataStore()
        {
            Data = new Dictionary<string, object>();
            EntityIdentities = new Dictionary<string, int>();
        }

        internal int GetNextId<TEntity>() where TEntity : class
        {
            string type = typeof(TEntity).ToString();

            if (!EntityIdentities.ContainsKey(type))
            {
                EntityIdentities.Add(type, 1);
            }
            else
            {
                EntityIdentities[type]++;
            }

            return EntityIdentities[type];
        }

        internal IQueryable<TEntity> GetData<TEntity>() where TEntity : class
        {
            string type = typeof (TEntity).ToString();

            if (!Data.ContainsKey(type))
            {
                var collection = new HashSet<TEntity>();
                Data.Add(type, collection);
                return collection.AsQueryable();
            }

            return ((HashSet<TEntity>)Data[type]).AsQueryable();
        }

        internal bool Remove<TEntity>(TEntity oldItem) where TEntity : class
        {
            return GetEntityHashSet<TEntity>().Remove(oldItem);
        }

        internal void Add<TEntity>(TEntity item) where TEntity : class
        {
            GetEntityHashSet<TEntity>().Add(item);
        }

        internal int Count<TEntity>() where TEntity : class
        {
            return GetEntityHashSet<TEntity>().Count();
        }

        internal void Clear<TEntity>() where TEntity : class
        {
            GetEntityHashSet<TEntity>().Clear();
        }

        internal IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class
        {
            return GetEntityHashSet<TEntity>().AsQueryable();
        }

        private HashSet<TEntity> GetEntityHashSet<TEntity>() where TEntity : class
        {
            string type = typeof(TEntity).ToString();

            if (!Data.ContainsKey(type))
            {
                var hashSet = new HashSet<TEntity>();
                Data.Add(type, hashSet);
                return hashSet;
            }

            return ((HashSet<TEntity>)Data[type]);
        }        
    }
}