using DevExtreme.AspNet.Data.ResponseModel;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerWriteableControllerTests<TController, TRepo, TEntity, TContext> :
        RepoTests<TRepo, TEntity, TContext>
        where TController : SqlServerWriteableController<TEntity,TContext>
        where TEntity : class, IHasSysUser, IHasIntegerId, new()
        where TContext : DbContext
        where TRepo : Repo<TEntity, TContext> {

        public SqlServerWriteableController<TEntity,TContext> Controller { get; }
        public SqlServerWriteableControllerTests(ITestOutputHelper output) : base(output) {
            var logger = NullLogger<SqlServerWriteableController<TEntity, TContext>>.Instance;
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
