using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{

    public abstract class ReadonlyTemporalRepoTests<TRepo, TEntity, TContext, THistoryContext> 
        : IClassFixture<ConfigurationFixture<TRepo>>

        where TEntity : class, IEFCoreTemporalModel , new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : ReadonlyTemporalRepo<TEntity, TContext, THistoryContext>         
        {


        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }

        public ReadonlyTemporalRepoTests(ITestOutputHelper output, ConfigurationFixture<TRepo> fixture) {

            Output = output;
            Repo = TestRepoFactory.CreateReadonlyTemporalRepo<TRepo,TEntity,TContext,THistoryContext, TRepo>(fixture);

        }
    }
}
