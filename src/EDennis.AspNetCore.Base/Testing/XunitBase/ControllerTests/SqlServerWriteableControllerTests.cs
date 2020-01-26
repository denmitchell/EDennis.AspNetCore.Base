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
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerWriteableControllerTests<TController, TRepo, TEntity, TContext> :
        RepoTests<TRepo, TEntity, TContext>
        where TController : SqlServerRepoController<TEntity,TContext>
        where TEntity : class, IHasSysUser, IHasIntegerId, new()
        where TContext : DbContext, ISqlServerDbContext<TContext>
        where TRepo : Repo<TEntity, TContext> {

        public SqlServerRepoController<TEntity,TContext> Controller { get; }
        public SqlServerWriteableControllerTests(ITestOutputHelper output) : base(output) {
            var logger = NullLogger<SqlServerRepoController<TEntity, TContext>>.Instance;
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
        public ExpectedActualList<Dictionary<string,object>> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select",Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter",Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort",Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip",Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take",Output);
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
        public string GetDynamicLinqResult(string where, string orderBy, string select, int? skip, int? take) {
            var iar = Controller.GetDynamicLinq(where, orderBy, select, skip, take);
            var data = ((string)(iar as ContentResult).Content);
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
        public ExpectedActualList<TEntity> GetDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var actualJson = GetDynamicLinqResult(where, orderBy, select, skip, take);
            var actual = JsonSerializer.Deserialize<List<TEntity>>(actualJson);

            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
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
        public async Task<string> GetDynamicLinqAsyncResult(string where, string orderBy, string select, int? skip, int? take) {
            var iar = await Controller.GetDynamicLinqAsync(where, orderBy, select, skip, take);
            var data = ((string)(iar as ContentResult).Content);
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
        public async Task<ExpectedActualList<TEntity>> GetDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);

            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var actualJson = await GetDynamicLinqAsyncResult(where, orderBy, select, skip, take);
            var actual = JsonSerializer.Deserialize<List<TEntity>>(actualJson);

            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
        }


        public ExpectedActual<TEntity> Get_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var actual = Controller.Get(id)
                .GetObject<TEntity>();

            return new ExpectedActual<TEntity> { Expected=expected, Actual=actual };
        }


        public async Task<ExpectedActual<TEntity>> GetAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var actual = (await Controller.GetAsync(id))
                .GetObject<TEntity>();

            return new ExpectedActual<TEntity> { Expected = expected, Actual = actual };
        }


        public ExpectedActual<List<TEntity>> Delete_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            Controller.Delete(id);

            var actual = Controller.Repo.Query.Where(x => x.Id >= start && x.Id <= end).ToList();

            return new ExpectedActual<List<TEntity>> { Expected = expected, Actual = actual };
        }


        public async Task<ExpectedActual<List<TEntity>>> DeleteAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            await Controller.DeleteAsync(id);

            var actual = Controller.Repo.Query.Where(x => x.Id >= start && x.Id <= end).ToList();

            return new ExpectedActual<List<TEntity>> { Expected = expected, Actual = actual };
        }

        public ExpectedActual<List<TEntity>> Post_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input");
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            Controller.Post(input);

            var actual = Controller.Repo.Query.Where(x => (x.Id >= start && x.Id <= end) || x.Id == input.Id).ToList();

            return new ExpectedActual<List<TEntity>> { Expected = expected, Actual = actual };
        }


        public async Task<ExpectedActual<List<TEntity>>> PostAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input");
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            await Controller.PostAsync(input);

            var actual = Controller.Repo.Query.Where(x => (x.Id >= start && x.Id <= end) || x.Id == input.Id).ToList();

            return new ExpectedActual<List<TEntity>> { Expected = expected, Actual = actual };
        }


        public ExpectedActual<List<TEntity>> Put_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<TEntity>("Input");
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            Controller.Put(input,id);

            var actual = Controller.Repo.Query.Where(x => x.Id >= start && x.Id <= end).ToList();

            return new ExpectedActual<List<TEntity>> { Expected = expected, Actual = actual };
        }

        public async Task<ExpectedActual<List<TEntity>>> PutAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<TEntity>("Input");
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var start = jsonTestCase.GetObject<int>("WindowStart");
            var end = jsonTestCase.GetObject<int>("WindowEnd");

            await Controller.PutAsync(input, id);

            var actual = Controller.Repo.Query.Where(x => x.Id >= start && x.Id <= end).ToList();

            return new ExpectedActual<List<TEntity>> { Expected = expected, Actual = actual };
        }

    }
}
