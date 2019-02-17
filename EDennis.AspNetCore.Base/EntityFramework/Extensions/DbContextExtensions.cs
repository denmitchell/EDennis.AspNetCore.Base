using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class DbContextExtensions {
        public static void SafeAttach<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class {
            try {
                context.Attach(entity);
            } catch {
                context.Entry(entity).State = EntityState.Detached;
                context.Attach(entity);
            }
        }
    }
}
