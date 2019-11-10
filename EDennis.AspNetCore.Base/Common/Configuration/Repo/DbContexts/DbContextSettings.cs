using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class DbContextSettings<TContext>
        where TContext: DbContext {
        public DatabaseProvider DatabaseProvider { get; set; } = DatabaseProvider.SqlServer;
        public TransactionType TransactionType { get; set; } = TransactionType.AutoCommit;
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
        public string ConnectionString { get; set; }
    }
}
