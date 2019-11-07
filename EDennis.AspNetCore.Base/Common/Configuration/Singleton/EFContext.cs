using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class EFContext {
        public string ProviderName { get; set; } = "SqlServer";
        public string ConnectionString { get; set; }
        public TransactionType TransactionType { get; set; } = TransactionType.AutoCommit;
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
    }
}
