using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnectionCache<TContext> : ConcurrentDictionary<string, CachedConnection<TContext>>
        where TContext : DbContext {

        public void SetDbContext(string instanceName,
            DbContextSettings<TContext> settings,
            DbContextProvider<TContext> dbContextProvider) {

            var cachedCxn = GetOrAdd(instanceName, (key)
                => new CachedConnection<TContext>());

            var context = DbContextProvider<TContext>.GetInterceptorContext(settings, cachedCxn);
            dbContextProvider.Context = context;

        }

    }
}