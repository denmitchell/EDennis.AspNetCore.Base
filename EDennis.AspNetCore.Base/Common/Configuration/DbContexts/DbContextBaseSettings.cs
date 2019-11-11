using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class DbContextBaseSettings<TContext>
        where TContext: DbContext {
        public DatabaseProvider DatabaseProvider { get; set; } = DatabaseProvider.SqlServer;
        public string ConnectionString { get; set; }
    }
}
