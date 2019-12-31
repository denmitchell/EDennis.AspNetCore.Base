using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class ResettableDbContext<T> : DbContext 
        where T : DbContext {

            public ResettableDbContext(DbContextOptionsProvider<T> provider) :
                base(provider.DbContextOptions) {
                if (!Database.IsInMemory() && provider.DisableAutoTransactions) {
                    Database.AutoTransactionsEnabled = false;
                    Database.UseTransaction(provider.Transaction as DbTransaction);
                }
            }

        }
    }
