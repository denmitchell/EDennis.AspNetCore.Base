using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{
    public abstract class WriteableTemporalRepoTests<TRepo, TEntity, TContext, THistoryContext> : IClassFixture<ConfigurationFixture<TRepo>>, IDisposable
        where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : WriteableTemporalRepo<TEntity, TContext, THistoryContext> {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        protected string InstanceName { get; }
        protected string HistoryInstanceName { get; }

        public WriteableTemporalRepoTests(ITestOutputHelper output, 
            ConfigurationFixture<TRepo> fixture,
            string testUser = "tester@example.org") {

            Output = output;
            Repo = TestRepoFactory.CreateWriteableTemporalRepo<TRepo, TEntity, TContext, THistoryContext, TRepo>(fixture, testUser) as TRepo;
            InstanceName = Repo.ScopeProperties.OtherProperties["InstanceName"].ToString();
            HistoryInstanceName = Repo.ScopeProperties.OtherProperties["HistoryInstanceName"].ToString();

        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    TestDbContextManager<TContext>.DropInMemoryDatabase(Repo.Context);
                    TestDbContextManager<THistoryContext>.DropInMemoryDatabase(Repo.HistoryContext);
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
