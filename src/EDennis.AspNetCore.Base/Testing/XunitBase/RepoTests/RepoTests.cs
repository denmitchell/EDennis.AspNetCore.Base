using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class RepoTests<TRepo, TEntity, TContext> 
            : IDisposable
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : Repo<TEntity, TContext> {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }
        protected TestRepoFactory<TRepo, TEntity, TContext> Factory { get; }

        public RepoTests(ITestOutputHelper output) {
            Output = output;
            Factory = new TestRepoFactory<TRepo, TEntity, TContext>();
            Repo = Factory.CreateRepo();
        }

        public void Dispose() {
            Factory.ResetRepo();
        }
    }
}
