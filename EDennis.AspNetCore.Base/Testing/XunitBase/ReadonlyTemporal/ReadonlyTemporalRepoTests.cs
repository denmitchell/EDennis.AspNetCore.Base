using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class ReadonlyTemporalRepoTests<TRepo, TEntity, TContext, THistoryContext> : IClassFixture<ConfigurationClassFixture>
        where TEntity : class, IEFCoreTemporalModel , new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : ReadonlyTemporalRepo<TEntity, TContext, THistoryContext> {

        protected readonly ITestOutputHelper _output;
        protected readonly TContext _context;
        protected readonly THistoryContext _histContext;
        protected readonly TRepo _repo;

        public ReadonlyTemporalRepoTests(ITestOutputHelper output, ConfigurationClassFixture configFixture) {
            _output = output;

            _context = TestDbContextManager<TContext>.GetReadonlyDatabase(
                configFixture.Configuration);

            _histContext = TestDbContextManager<THistoryContext>.GetReadonlyDatabase(
                configFixture.Configuration);


            //using reflection, instantiate the repo
            _repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { _context, _histContext }) as TRepo;

        }
    }
}
