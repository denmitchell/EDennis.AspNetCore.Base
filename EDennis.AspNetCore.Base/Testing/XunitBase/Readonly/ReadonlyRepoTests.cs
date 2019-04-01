using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class ReadonlyRepoTests<TRepo, TEntity, TContext> 
            : IClassFixture<ConfigurationClassFixture<TRepo>>

        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : ReadonlyRepo<TEntity, TContext> 
        
        {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }

        public ReadonlyRepoTests(ITestOutputHelper output, ConfigurationClassFixture<TRepo> fixture) {

            Output = output;
            Repo = TestRepoFactory.CreateReadonlyRepo<TRepo, TEntity, TContext>(fixture);

        }


    }
}
