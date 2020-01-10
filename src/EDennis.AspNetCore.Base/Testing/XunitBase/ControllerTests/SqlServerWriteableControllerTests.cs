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
    public abstract class SqlServerWriteableControllerTests<TController, TRepo, TEntity, TContext> :
        RepoTests<TRepo, TEntity, TContext>
        where TController : SqlServerWriteableController<TEntity,TContext>, new()
        where TEntity : class, IHasSysUser, IHasIntegerId, new()
        where TContext : DbContext
        where TRepo : Repo<TEntity, TContext> {

        public SqlServerWriteableController<TEntity,TContext> Controller { get; }
        public SqlServerWriteableControllerTests(ITestOutputHelper output) : base(output) {
            var logger = NullLogger<SqlServerWriteableController<TEntity, TContext>>.Instance;
            Activator.CreateInstance(typeof(TController), new object[] { Repo, logger});
        }

    }
}
