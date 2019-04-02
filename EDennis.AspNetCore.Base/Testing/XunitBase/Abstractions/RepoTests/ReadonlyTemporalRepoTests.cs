using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class ReadonlyTemporalRepoTests<TRepo, TEntity, TContext, THistoryContext> 
        : IClassFixture<ConfigurationFactory<TRepo>>

        where TEntity : class, IEFCoreTemporalModel , new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : ReadonlyTemporalRepo<TEntity, TContext, THistoryContext>         
        {


        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }

        public ReadonlyTemporalRepoTests(ITestOutputHelper output, ConfigurationFactory<TRepo> fixture) {

            Output = output;
            Repo = TestRepoFactory.CreateReadonlyTemporalRepo<TRepo,TEntity,TContext,THistoryContext>(fixture);

        }
    }
}
