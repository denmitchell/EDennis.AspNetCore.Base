using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class ReadonlyRepoTests<TRepo, TEntity, TContext> : IClassFixture<ConfigurationClassFixture<TRepo>>
        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : ReadonlyRepo<TEntity, TContext> {

        protected ConfigurationClassFixture<TRepo> _fixture;

        protected ITestOutputHelper Output { get; }
        protected TContext Context { get; }
        protected TRepo Repo { get; }
        protected string InstanceName { get; } = "readonly";

        public ReadonlyRepoTests(ITestOutputHelper output, ConfigurationClassFixture<TRepo> fixture) {

            _fixture = fixture;

            Output = output;

            Context = TestDbContextManager<TContext>.GetReadonlyDatabase(
                fixture.Configuration);

            //using reflection, instantiate the repo
            Repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { Context }) as TRepo;

        }


    }
}
