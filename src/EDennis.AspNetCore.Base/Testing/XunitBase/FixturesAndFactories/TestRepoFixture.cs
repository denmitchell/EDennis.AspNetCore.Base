using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;


namespace EDennis.AspNetCore.Base.Testing {
    public class TestRepoFixture<TRepo, TEntity, TContext> : IDisposable
        where TRepo : IRepo<TEntity, TContext>
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext //ResettableDbContext<TContext> 
        {

        public TestRepoFactory<TRepo,TEntity,TContext> Factory { get; }
        public TRepo Repo { get; }

        public TestRepoFixture(){
            Factory = new TestRepoFactory<TRepo,TEntity,TContext>();
            Repo = Factory.CreateRepo();
        }

        public void Dispose() {
            Factory.ResetRepo();
        }

    }
}
