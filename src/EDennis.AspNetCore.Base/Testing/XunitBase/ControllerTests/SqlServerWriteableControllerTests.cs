using DevExtreme.AspNet.Data.ResponseModel;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
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


        /// <summary>
        /// Use this method for testing if you want full control over
        /// entry of the parameters to the controller method.
        /// </summary>
        /// <param name="select"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="totalSummary"></param>
        /// <param name="group"></param>
        /// <param name="groupSummary"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Returns actual and expected results from GetDevExtreme.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, Filter, Sort, Skip, and Take
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public FlatExpectedActualList GetDevExtreme_ExpectedActual(JsonTestCase jsonTestCase, ITestOutputHelper output) {

            var select = jsonTestCase.GetObjectOrDefault<string>("Select",output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter",output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort",output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip",output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take",output);
            if (take == default)
                take = int.MaxValue;

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = GetDevExtremeResult(select, sort, filter, skip, take, null, null, null);
            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            return new FlatExpectedActualList { Expected = expected, Actual = actual };
        }


        /// <summary>
        /// Use this method for testing if you want full control over
        /// entry of the parameters to the controller method.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="select"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<dynamic> GetDynamicLinqResult(string where, string orderBy, string select, int? skip, int? take) {
            var iar = Controller.GetDynamicLinq(where, orderBy, select, skip, take);
            var data = ((List<dynamic>)(iar as ObjectResult).Value);
            return data;
        }


        /// <summary>
        /// Returns actual and expected results from GetDynamicLinq.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Where, OrderBy, Select, Skip, and Take
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public FlatExpectedActualList GetDynamicLinq_ExpectedActual(JsonTestCase jsonTestCase, ITestOutputHelper output) {

            var where = jsonTestCase.GetObjectOrDefault<string>("Where", output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", output);

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = GetDynamicLinqResult(where, orderBy, select, skip, take);
            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            return new FlatExpectedActualList { Expected = expected, Actual = actual };
        }


    }
}
