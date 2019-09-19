using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestDbContextOptionsCache<TContext> : 
            Dictionary<string,DbContextOptions<TContext>>
        where TContext : DbContext { }
}
