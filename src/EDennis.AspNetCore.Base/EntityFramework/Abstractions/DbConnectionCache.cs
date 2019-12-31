using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnectionCache<TContext> : 
            ConcurrentDictionary<string,DbConnection<TContext>>
        where TContext : DbContext { 


        public DbConnection<TContext> GetOrAdd(string instance, 
            DbContextInterceptorSettings<TContext> settings,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider) {

            var cachedCxn = GetOrAdd(instance, (key)
                 => DbConnectionManager.GetDbConnection(settings));

            dbContextOptionsProvider.DbContextOptions = cachedCxn.DbContextOptionsBuilder.Options;
            dbContextOptionsProvider.DisableAutoTransactions = true;
            dbContextOptionsProvider.Transaction = cachedCxn.IDbTransaction;

            return cachedCxn;

        }

    }
}
