using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class SqlServerReadonlyEndpointTests<TProgram, TLauncher> : LauncherEndpointTests<TProgram, TLauncher>
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {

        public SqlServerReadonlyEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
        }

        /// <summary>
        /// Returns actual and expected results from GetDevExtreme.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, Filter, Sort, Skip, Take, 
        ///    and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActualList<Dictionary<string, object>> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return GetDevExtreme_ExpectedActual_Base(t,jsonTestCase, u => HttpClient.Get<List<dynamic>>(u));
        }


        /// <summary>
        /// Returns actual and expected results from GetDevExtreme.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, Filter, Sort, Skip, Take, 
        ///    and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task<ExpectedActualList<Dictionary<string, object>>> GetDevExtremeAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() => 
                GetDevExtreme_ExpectedActual_Base(t, jsonTestCase, u => HttpClient.GetAsync<List<dynamic>>(u).Result));            
        }


        private ExpectedActualList<Dictionary<string, object>> GetDevExtreme_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, Func<string,dynamic> getMethod) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            if (take == default)
                take = int.MaxValue;

            var queryStrings = new List<string>();

            if (select != default)
                queryStrings.Add($"select={select}");
            else if (filter != default)
                queryStrings.Add($"filter={filter}");
            else if (sort != default)
                queryStrings.Add($"sort={sort}");

            queryStrings.Add($"skip={skip}");
            queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/devextreme?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamicResult = getMethod(url);
            var statusCode = actualDynamicResult.GetStatusCode();

            List<dynamic> actualDynamic;
            if (statusCode > 299)
                actualDynamic = new List<dynamic> { new {
                    StatusCode = statusCode,
                    Text = actualDynamicResult.GetObject<string>() }
                };
            else
                actualDynamic = actualDynamicResult.GetObject<List<dynamic>>();

            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            return new ExpectedActualList<Dictionary<string, object>> { Expected = expected, Actual = actual };
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
