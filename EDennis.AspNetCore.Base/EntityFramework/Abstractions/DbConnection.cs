using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnection<TContext>
        where TContext: DbContext {
        public DbContextOptionsBuilder<TContext> DbContextOptionsBuilder { get; set; }
        public IDbConnection IDbConnection { get; set; }
        public IDbTransaction IDbTransaction { get; set; }

    }
}
