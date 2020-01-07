using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class TemporalRepoTests<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> 
        : IDisposable
        where TEntity : class, IEFCoreTemporalModel, new()
        where THistoryEntity: class, TEntity, new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TTemporalRepo : TemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> {

        protected ITestOutputHelper Output { get; }
        protected TTemporalRepo Repo { get; }
        protected TestTemporalRepoFactory<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> Factory { get; }
        public TemporalRepoTests(ITestOutputHelper output) {
            Output = output;
            Factory = new TestTemporalRepoFactory<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>();
            Repo = Factory.CreateRepo();
        }

        public void Dispose() {
            Factory.ResetRepo();
        }
    }
}
