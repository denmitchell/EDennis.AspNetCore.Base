using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Testing;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerReadonlyControllerTests<TController, TRepo, TEntity, TContext> :
        RepoTests<TRepo, TEntity, TContext>
        where TController : SqlServerReadonlyController<TEntity,TContext>
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : Repo<TEntity, TContext> {

        public SqlServerReadonlyController<TEntity,TContext> Controller { get; }
        public SqlServerReadonlyControllerTests(ITestOutputHelper output) : base(output) {
            var logger = NullLogger<SqlServerReadonlyController<TEntity, TContext>>.Instance;
            Controller = (TController)Activator.CreateInstance(typeof(TController), new object[] { Repo, logger});
        }


        public List<dynamic> GetDevExtremeResult(string select, string sort, string filter, int skip, int take, string totalSummary, string group, string groupSummary) {
            var iar = Controller.GetDevExtreme(select, sort, filter, skip, take, totalSummary, group, groupSummary);
            var data = ((LoadResult)(iar as ObjectResult).Value).data;

            IEnumerable<dynamic> ToGeneric() {
                foreach (var datum in data) {
                    yield return datum;
                }
            }
            return ToGeneric().ToList();
        }

    }
}
