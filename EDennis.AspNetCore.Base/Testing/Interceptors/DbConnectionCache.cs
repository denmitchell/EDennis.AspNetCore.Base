using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace EDennis.AspNetCore.Base.Testing {

    public class DbConnection<TContext>
        where TContext: DbContext {
        public DbContextOptions<TContext> DbContextOptions { get; set; }
        public IDbConnection IDbConnection { get; set; }
        public IDbTransaction IDbTransaction { get; set; }

    }
    public class DbConnectionCache<TContext> : 
            Dictionary<string,DbConnection<TContext>>
        where TContext : DbContext { }
}
