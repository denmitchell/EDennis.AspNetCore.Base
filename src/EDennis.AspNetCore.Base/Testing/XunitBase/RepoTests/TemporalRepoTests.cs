using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class TemporalRepoTests<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> 
            : IClassFixture<TestTemporalRepoFixture<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>>, IDisposable
        where TEntity : class, IEFCoreTemporalModel, new()
        where THistoryEntity: TEntity
        where TContext : DbContext
        where THistoryContext : DbContext
        where TTemporalRepo : TemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext> {

        protected ITestOutputHelper Output { get; }
        protected TTemporalRepo Repo { get; }
        private readonly TestTemporalRepoFixture<TTemporalRepo, TEntity, THistoryEntity,TContext,THistoryContext> _fixture;

        public TemporalRepoTests(ITestOutputHelper output, TestTemporalRepoFixture<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> fixture) {
            Output = output;
            Repo = fixture.Repo;
            _fixture = fixture;
        }

        public void Dispose() {
            _fixture.Dispose();
        }
    }
}
