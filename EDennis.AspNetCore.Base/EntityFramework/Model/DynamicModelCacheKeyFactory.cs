using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class replaces the default IModelCacheKeyFactory
    /// for DbContexts when an in-memory database is used
    /// </summary>
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory {
        public object Create(DbContext context) {
            if (context is DbContextBase dynamicContext) {
                return (context.GetType(), dynamicContext.Database.IsInMemory());
            }
            return context.GetType();
        }
    }

}
