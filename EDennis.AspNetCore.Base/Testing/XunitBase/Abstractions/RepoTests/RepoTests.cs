using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class RepoTests<TRepo, TEntity, TContext> 
            : IClassFixture<RepoFixture<TRepo, TEntity, TContext>>, IDisposable
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : Repo<TEntity, TContext> {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        protected RepoFixture<TRepo, TEntity, TContext> Fixture { get; }

        public RepoTests(ITestOutputHelper output, RepoFixture<TRepo, TEntity, TContext> fixture) {
            Output = output;
            Repo = fixture.Repo;
            Fixture = fixture;
        }

        public void Dispose() {
            Fixture.Reset();
        }
    }
}
