using EDennis.AspNetCore.Base.EntityFramework;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class RepoTests<TRepo, TEntity, TContext> 
            : IClassFixture<TestRepoFixture<TRepo, TEntity, TContext>>, IDisposable
        where TEntity : class, IHasSysUser, new()
        where TContext : ResettableDbContext<TContext>
        where TRepo : Repo<TEntity, TContext> {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        private readonly TestRepoFixture<TRepo, TEntity, TContext> _fixture;

        public RepoTests(ITestOutputHelper output, TestRepoFixture<TRepo, TEntity, TContext> fixture) {
            Output = output;
            Repo = fixture.Repo;
            _fixture = fixture;
        }

        public void Dispose() {
            _fixture.Dispose();
        }
    }
}
