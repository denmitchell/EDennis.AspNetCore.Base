using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class ReadonlyRepoTests<TRepo, TEntity, TContext> 
            : IClassFixture<ConfigurationFixture<TRepo>>

        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : ReadonlyRepo<TEntity, TContext> 
        
        {

        protected ITestOutputHelper Output { get; }
        protected TRepo Repo { get; }

        public ReadonlyRepoTests(ITestOutputHelper output, ConfigurationFixture<TRepo> fixture) {

            Output = output;
            Repo = TestRepoFactory.CreateRepo<TRepo, TEntity, TContext, TRepo>(fixture);

        }


    }
}
