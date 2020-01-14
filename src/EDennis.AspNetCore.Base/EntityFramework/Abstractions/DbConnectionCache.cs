using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnectionCache<TContext> : ConcurrentDictionary<string, CachedConnection<TContext>>
        where TContext : DbContext {

        private readonly StoredProcedureDefs<TContext> _spDefs;

        public DbConnectionCache(StoredProcedureDefs<TContext> spDefs){
            _spDefs = spDefs;
        }

        public void SetDbContext(string instanceName,
            DbContextSettings<TContext> settings,
            DbContextProvider<TContext> dbContextProvider) {

            var cachedCxn = GetOrAdd(instanceName, (key)
                => new CachedConnection<TContext>());

            var context = DbContextProvider<TContext>.GetInterceptorContext(settings, cachedCxn);

            if (typeof(ISqlServerDbContext<TContext>).IsAssignableFrom(typeof(TContext))) {
                (context as ISqlServerDbContext<TContext>).StoredProcedureDefs = _spDefs;
            }


            dbContextProvider.Context = context;

        }

    }
}