using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerReadonlyControllerTests<TController, TRepo, TEntity, TContext> :
        RepoTests<TRepo, TEntity, TContext>
        where TController : SqlServerReadonlyController<TEntity,TContext>, new()
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : Repo<TEntity, TContext> {

        public SqlServerReadonlyController<TEntity,TContext> Controller { get; }
        public SqlServerReadonlyControllerTests(ITestOutputHelper output) : base(output) {
            var logger = NullLogger<SqlServerReadonlyController<TEntity, TContext>>.Instance;
            Activator.CreateInstance(typeof(TController), new object[] { Repo, logger});
        }

    }
}
