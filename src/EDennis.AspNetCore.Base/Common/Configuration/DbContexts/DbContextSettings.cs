using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base {
    public class DbContextSettings<TContext> : DbContextBaseSettings<TContext>
        where TContext: DbContext {
        public DbContextInterceptorSettings<TContext> Interceptor { get; set; }

    }
}
