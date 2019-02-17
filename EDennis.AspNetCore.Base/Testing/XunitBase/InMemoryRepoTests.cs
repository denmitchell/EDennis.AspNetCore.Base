using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class InMemoryRepoTests<TRepo, TEntity, TContext> : IClassFixture<ConfigurationClassFixture>, IDisposable
        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : SqlRepo<TEntity, TContext> {

        protected readonly ITestOutputHelper _output;
        protected readonly string _dbName;
        protected readonly TContext _context;
        protected readonly TRepo _repo;

        public InMemoryRepoTests(ITestOutputHelper output, ConfigurationClassFixture configFixture) {

            _output = output;
            var dbName = configFixture.Configuration.GetDatabaseName<TContext>(); 
            var instanceName = Guid.NewGuid().ToString();

            _context = TestDbContextManager<TContext>
                .CreateInMemoryDatabase(dbName,instanceName);

            //using reflection, instantiate the repo
            _repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { _context }) as TRepo;

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    TestDbContextManager<TContext>.DropInMemoryDatabase(_context);
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
