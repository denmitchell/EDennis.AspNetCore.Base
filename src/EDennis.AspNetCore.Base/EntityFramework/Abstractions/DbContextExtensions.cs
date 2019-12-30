using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class DbContextExtensions {
        public static void SaveAndUntrack<TEntity,TContext>(this TContext context, TEntity entity)
            where TContext: DbContext
            where TEntity : class {
            context.SaveChanges();
            if (entity != null)
                context.Entry(entity).State = EntityState.Detached;
        }

        public static async Task SaveAndUntrackAsync<TEntity, TContext>(this TContext context, TEntity entity)
            where TContext : DbContext
            where TEntity : class {
            await context.SaveChangesAsync();
            if (entity != null)
                context.Entry(entity).State = EntityState.Detached;
            return;
        }
    }
}
