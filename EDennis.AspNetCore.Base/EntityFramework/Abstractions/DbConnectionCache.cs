using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class DbConnectionCache<TContext> : 
            ConcurrentDictionary<string,DbConnection<TContext>>
        where TContext : DbContext { 
    
    }
}
