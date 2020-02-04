using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class CachedConnection<TContext>
        where TContext : DbContext {
        public DbConnection DbConnection { get; set; }
        public DbTransaction DbTransaction { get; set; }
        public string InstanceName { get; set; }
    }

}
