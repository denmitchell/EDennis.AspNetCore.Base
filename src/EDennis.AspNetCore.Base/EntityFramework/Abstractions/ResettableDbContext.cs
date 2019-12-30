using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EDennis.AspNetCore.Base.EntityFramework.Abstractions {
    public class ResettableDbContext : DbContext {

            public ResettableDbContext(DbContextOptionsProvider<ResettableDbContext> provider) :
                base(provider.DbContextOptions) {
                if (!Database.IsInMemory() && provider.DisableAutoTransactions) {
                    Database.AutoTransactionsEnabled = false;
                    Database.UseTransaction(provider.Transaction as DbTransaction);
                }
            }

        }
    }
