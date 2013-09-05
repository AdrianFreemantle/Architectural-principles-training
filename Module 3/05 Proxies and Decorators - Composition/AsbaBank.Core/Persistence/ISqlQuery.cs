using System;
using System.Collections;
using System.Collections.Generic;

namespace AsbaBank.Core.Persistence
{
    public interface ISqlQuery
    {
        IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters);
        IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters);
    }
}