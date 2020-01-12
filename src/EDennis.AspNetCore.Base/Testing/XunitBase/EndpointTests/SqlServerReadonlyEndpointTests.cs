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
    public abstract class SqlServerReadonlyEndpointTests<TProgram, TLauncher> : LauncherEndpointTests<TProgram, TLauncher>
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
        private ExpectedActualList<Dictionary<string, object>> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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
            if (filter != default)
                queryStrings.Add($"filter={filter}");
            if (sort != default)
                queryStrings.Add($"sort={sort}");

            queryStrings.Add($"skip={skip}");
            queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/devextreme?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamicResult = HttpClient.Get<List<dynamic>>(url);
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
        /// Returns actual and expected results from GetDevExtreme.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, Filter, Sort, Skip, Take, 
        ///    and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActualList<Dictionary<string, object>> GetDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return GetDynamicLinq_ExpectedActual_Base(t, jsonTestCase, false);
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
        public async Task<ExpectedActualList<Dictionary<string, object>>> GetDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                GetDynamicLinq_ExpectedActual_Base(t, jsonTestCase, true));
        }


        /// <summary>
        /// Returns actual and expected results from GetDynamicLinq.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Where, OrderBy, Select, Skip, Take, and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private ExpectedActualList<Dictionary<string, object>> GetDynamicLinq_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var queryStrings = new List<string>();

            if (where != default)
                queryStrings.Add($"where={where}");
            if (orderBy != default)
                queryStrings.Add($"orderBy={orderBy}");
            if (select != default)
                queryStrings.Add($"select={select}");
            if (skip != default)
                queryStrings.Add($"skip={skip}");
            if (take != default)
                queryStrings.Add($"take={take}");

            var url = $"{controllerPath}/linq{(isAsync ? "/async" : "")}?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualDynamicResult;
            if (isAsync)
                actualDynamicResult = HttpClient.GetAsync<List<dynamic>>(url).Result;
            else
                actualDynamicResult = HttpClient.Get<List<dynamic>>(url);

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
        /// Returns actual and expected results from GetOData.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// Select, OrderBy, Filter, Skip, Top, and ControllerPath
        /// Because it returns property dictionary lists for expected and 
        /// actual, the method cannot be used with the Expand option
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private ExpectedActualList<Dictionary<string, object>> GetOData_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var top = jsonTestCase.GetObjectOrDefault<int?>("Top", Output);
            if (top == default)
                top = int.MaxValue;

            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);

            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");
            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var queryStrings = new List<string>();

            if (select != default)
                queryStrings.Add($"select={select}");
            if (filter != default)
                queryStrings.Add($"where={filter}");
            if (orderBy != default)
                queryStrings.Add($"orderBy={orderBy}");

            queryStrings.Add($"skip={skip}");
            queryStrings.Add($"top={top}");

            var url = $"{controllerPath}/odata?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualDynamicResult;
            actualDynamicResult = HttpClient.Get<List<dynamic>>(url);

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


    }
}
