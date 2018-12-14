using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Testing {

    public abstract class TransactionTest<TContext> : IDisposable 
        where TContext : DbContextBase {

        protected TContext Context;
        protected DbContextBaseTestCache dbContextBaseRepo;
        protected string NamedInstance;
        protected string ConnectionStringName;

        public TransactionTest() {            
            IConfiguration config =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            string ConnectionStringName = typeof(TContext).Name;

            ILogger<DbContextBaseTestCache> logger = new LoggerFactory().CreateLogger<DbContextBaseTestCache>();
            dbContextBaseRepo = new DbContextBaseTestCache(config,logger);

            NamedInstance = Guid.NewGuid().ToString();
            dbContextBaseRepo.GetOrAddTestingTransactionContexts(NamedInstance);

            var rec = dbContextBaseRepo.GetDbContexts(NamedInstance)[ConnectionStringName];
            Context = rec as TContext;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    dbContextBaseRepo
                        .DropTestingTransactionContexts(NamedInstance);
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion


    }
}
