using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class ReadonlyTemporalRepoTests<TRepo, TEntity, TContext, THistoryContext> : IClassFixture<ConfigurationClassFixture<TRepo>>
        where TEntity : class, IEFCoreTemporalModel , new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : ReadonlyTemporalRepo<TEntity, TContext, THistoryContext> {

        protected readonly ConfigurationClassFixture<TRepo> _fixture;

        protected ITestOutputHelper Output { get; }
        protected TContext Context { get; }
        protected THistoryContext HistoryContext { get; }
        protected TRepo Repo { get; }
        protected string InstanceName { get; } = "readonly-temporal";

        public ReadonlyTemporalRepoTests(ITestOutputHelper output, ConfigurationClassFixture<TRepo> fixture) {

            _fixture = fixture;

            Output = output;

            Context = TestDbContextManager<TContext>.GetReadonlyDatabase(
                fixture.Configuration);

            HistoryContext = TestDbContextManager<THistoryContext>.GetReadonlyDatabase(
                fixture.Configuration);


            //using reflection, instantiate the repo
            Repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { Context, HistoryContext }) as TRepo;

        }
    }
}
