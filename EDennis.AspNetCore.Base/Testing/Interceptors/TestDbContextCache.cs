using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestDbContextCache<TContext> : Dictionary<string,TContext>
        where TContext : DbContext { }
}
