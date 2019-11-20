using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class DbContextSettings<TContext> : DbContextBaseSettings<TContext>
        where TContext: DbContext {
        public DbContextInterceptorSettings<TContext> Interceptor { get; set; }
    }
}
