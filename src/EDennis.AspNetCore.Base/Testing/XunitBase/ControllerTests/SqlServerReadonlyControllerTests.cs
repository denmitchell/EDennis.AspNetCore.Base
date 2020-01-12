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
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Threading.Tasks;
using EDennis.NetCoreTestingUtilities;

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
        public ExpectedActualList<Dictionary<string, object>> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take", Output);
            if (take == default)
                take = int.MaxValue;

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = GetDevExtremeResult(select, sort, filter, skip, take, null, null, null);
            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            return new ExpectedActualList<Dictionary<string, object>> { Expected = expected, Actual = actual };
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
        public ExpectedActualList<Dictionary<string, object>> GetDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = GetDynamicLinqResult(where, orderBy, select, skip, take);
            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            return new ExpectedActualList<Dictionary<string, object>> { Expected = expected, Actual = actual };
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
        public async Task<List<dynamic>> GetDynamicLinqAsyncResult(string where, string orderBy, string select, int? skip, int? take) {
            var iar = await Controller.GetDynamicLinqAsync(where, orderBy, select, skip, take);
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
        public async Task<ExpectedActualList<Dictionary<string, object>>> GetDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = await GetDynamicLinqAsyncResult(where, orderBy, select, skip, take);
            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            return new ExpectedActualList<Dictionary<string, object>> { Expected = expected, Actual = actual };
        }






    }
}
