using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class WriteableRepoTests<TRepo, TEntity, TContext> : IClassFixture<ConfigurationClassFixture<TRepo>>, IDisposable
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : WriteableRepo<TEntity, TContext> {

        protected ConfigurationClassFixture<TRepo> _fixture;

        protected ITestOutputHelper Output { get; }
        protected TContext Context { get; }
        protected TRepo Repo { get; }
        protected string DatabaseName { get; }
        protected string InstanceName { get; }

        protected ScopeProperties ScopeProperties { get; }

        public WriteableRepoTests(ITestOutputHelper output, 
            ConfigurationClassFixture<TRepo> fixture,
            string testUser = "moe@stooges.org") {

            _fixture = fixture;

            Output = output;
            DatabaseName = fixture.Configuration.GetDatabaseName<TContext>(); 
            InstanceName = Guid.NewGuid().ToString();

            Context = TestDbContextManager<TContext>
                .CreateInMemoryDatabase(DatabaseName,InstanceName);

            ScopeProperties = new ScopeProperties {
                User = testUser
            };

            //using reflection, instantiate the repo
            Repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { Context, ScopeProperties }) as TRepo;

        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    TestDbContextManager<TContext>.DropInMemoryDatabase(Context);
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
