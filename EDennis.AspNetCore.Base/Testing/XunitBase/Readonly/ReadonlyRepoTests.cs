using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class ReadonlyRepoTests<TRepo, TEntity, TContext> : IClassFixture<ConfigurationClassFixture>
        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : ReadonlyRepo<TEntity, TContext> {

        protected readonly ITestOutputHelper _output;
        protected readonly TContext _context;
        protected readonly TRepo _repo;

        public ReadonlyRepoTests(ITestOutputHelper output, ConfigurationClassFixture configFixture) {
            _output = output;

            _context = TestDbContextManager<TContext>.GetReadonlyDatabase(
                configFixture.Configuration);

            //using reflection, instantiate the repo
            _repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { _context }) as TRepo;

        }
    }
}
