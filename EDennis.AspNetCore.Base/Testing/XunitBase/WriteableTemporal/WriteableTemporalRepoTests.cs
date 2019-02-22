using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class WriteableTemporalRepoTests<TRepo, TEntity, TContext, THistoryContext> : IClassFixture<ConfigurationClassFixture>, IDisposable
        where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : WriteableTemporalRepo<TEntity, TContext, THistoryContext> {

        protected readonly ITestOutputHelper _output;
        protected readonly string _dbName;
        protected readonly string _dbHistName;
        protected readonly TContext _context;
        protected readonly THistoryContext _histContext;
        protected readonly TRepo _repo;
        protected readonly string _instanceName;
        protected readonly string _histInstanceName;

        public WriteableTemporalRepoTests(ITestOutputHelper output, ConfigurationClassFixture configFixture) {

            _output = output;
            _dbName = configFixture.Configuration.GetDatabaseName<TContext>(); 
            _instanceName = Guid.NewGuid().ToString();

            _context = TestDbContextManager<TContext>
                .CreateInMemoryDatabase(_dbName,_instanceName);

            _dbHistName = configFixture.Configuration.GetDatabaseName<THistoryContext>();
            _histInstanceName = _instanceName + "-hist";

            _histContext = TestDbContextManager<THistoryContext>
                .CreateInMemoryDatabase(_dbHistName, _histInstanceName);


            //using reflection, instantiate the repo
            _repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { _context, _histContext }) as TRepo;

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
