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

        protected readonly ConfigurationClassFixture _fixture;

        protected ITestOutputHelper Output { get; }
        protected string DatabaseName { get; }
        protected string HistoryDatabaseName { get; }
        protected TContext Context { get; }
        protected THistoryContext HistoryContext { get; }
        protected TRepo Repo { get; }
        protected string InstanceName { get; }
        protected string HistoryInstanceName { get; }

        public WriteableTemporalRepoTests(ITestOutputHelper output, ConfigurationClassFixture fixture) {

            _fixture = fixture;

            Output = output;
            DatabaseName = fixture.Configuration.GetDatabaseName<TContext>(); 
            InstanceName = Guid.NewGuid().ToString();

            Context = TestDbContextManager<TContext>
                .CreateInMemoryDatabase(DatabaseName, InstanceName);

            HistoryDatabaseName = fixture.Configuration.GetDatabaseName<THistoryContext>();
            HistoryInstanceName = InstanceName + "-hist";

            HistoryContext = TestDbContextManager<THistoryContext>
                .CreateInMemoryDatabase(HistoryDatabaseName, HistoryInstanceName);


            //using reflection, instantiate the repo
            Repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { Context, HistoryContext }) as TRepo;

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    TestDbContextManager<TContext>.DropInMemoryDatabase(Context);
                    TestDbContextManager<THistoryContext>.DropInMemoryDatabase(HistoryContext);
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
