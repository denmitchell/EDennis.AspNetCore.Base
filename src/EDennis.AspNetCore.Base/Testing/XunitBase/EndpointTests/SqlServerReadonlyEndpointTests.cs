using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerReadonlyEndpointTests<TEntity, TProgram, TLauncher> 
        : LauncherEndpointTests<TProgram, TLauncher>
        where TEntity : class, new()
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
        public ExpectedActualList<TEntity> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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

            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var actualLoadResult = HttpClient.Get<DeserializableLoadResult<TEntity>>(url);
            var statusCode = actualLoadResult.GetStatusCode();

            List<TEntity> actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode={statusCode},Text={actualLoadResult.GetObject<string>()}");
            else
                actual = actualLoadResult.GetObject<DeserializableLoadResult<TEntity>>().data.ToList();

            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
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
        public ExpectedActual<PagedResult<dynamic>> GetDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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
        public async Task<ExpectedActual<PagedResult<dynamic>>> GetDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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
        private ExpectedActual<PagedResult<dynamic>> GetDynamicLinq_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);

            var expected = jsonTestCase.GetObject<PagedResult<dynamic>,TEntity>("Expected");

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

            IActionResult actualResult;
            if (isAsync)
                actualResult = HttpClient.GetAsync<PagedResult<dynamic>,TEntity>(url).Result;
            else
                actualResult = HttpClient.Get<PagedResult<dynamic>, TEntity>(url);


            var statusCode = actualResult.GetStatusCode();
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");

            var actual = actualResult.GetObject<PagedResult<dynamic>>();

            return new ExpectedActual<PagedResult<dynamic>> { Expected = expected, Actual = actual };
        }



        /*
         * ODATA REQUIRES WORKAROUNDS IN .NET CORE 3.1 AND BREAKS SWAGGER UI

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
        public ExpectedActualList<TEntity> GetOData_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var top = jsonTestCase.GetObjectOrDefault<int?>("Top", Output);
            if (top == default)
                top = int.MaxValue;

            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);

            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var queryStrings = new List<string>();

            if (select != default)
                queryStrings.Add($"$select={select}");
            if (filter != default)
                queryStrings.Add($"$where={filter}");
            if (orderBy != default)
                queryStrings.Add($"$orderBy={orderBy}");

            queryStrings.Add($"$skip={skip}");
            queryStrings.Add($"$top={top}");

            var url = $"{controllerPath}/odata?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            actualResult = HttpClient.Get<List<dynamic>>(url);

            var statusCode = actualResult.GetStatusCode();

            List<TEntity> actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");
            else
                actual = actualResult.GetObject<List<TEntity>>();


            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
        }

        */

        /// <summary>
        /// Returns actual and expected results from GetFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActualList<TEntity> GetListFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return GetListFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, false, false);
        }


        /// <summary>
        /// Returns actual and expected results from GetFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task<ExpectedActualList<TEntity>> GetListFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                GetListFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, true, false));
        }




        /// <summary>
        /// Returns actual and expected results from GetJsonColumnFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActualList<TEntity> GetListFromJsonStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return GetListFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, false, true);
        }


        /// <summary>
        /// Returns actual and expected results from GetJsonColumnFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task<ExpectedActualList<TEntity>> GetListFromJsonStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                GetListFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, true, true));
        }


        private ExpectedActualList<TEntity> GetListFromStoredProcedure_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync, bool isJsonColumn) {
            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var paramValues = jsonTestCase.GetObject<Dictionary<string,string>>("ParamValues", Output);

            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);

            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var queryStrings = new List<string> {
                $"spName={spName}"
            };
            foreach (var key in paramValues.Keys)
                queryStrings.Add($"{key}={paramValues[key]}");

            var url = $"{controllerPath}/{(isJsonColumn ? "json": "sp")}{(isAsync ? "/async" : "")}?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            if (isAsync)
                actualResult = HttpClient.GetAsync<List<TEntity>>(url).Result;
            else
                actualResult = HttpClient.Get<List<TEntity>>(url);

            var statusCode = actualResult.GetStatusCode();

            List<TEntity> actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");
            else
                actual = actualResult.GetObject<List<TEntity>>();


            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
        }




        /// <summary>
        /// Returns actual and expected results from GetFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActual<TEntity> GetSingleFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return GetSingleFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, false, false);
        }


        /// <summary>
        /// Returns actual and expected results from GetFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task<ExpectedActual<TEntity>> GetSingleFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                GetSingleFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, true, false));
        }




        /// <summary>
        /// Returns actual and expected results from GetJsonColumnFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ExpectedActual<TEntity> GetSingleFromJsonStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return GetSingleFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, false, true);
        }


        /// <summary>
        /// Returns actual and expected results from GetJsonColumnFromStoredProcedure.
        /// Note: this method looks for the following optional TestJson
        /// parameters (case sensitive):
        /// SpName, ParamValues and ControllerPath
        /// </summary>
        /// <param name="jsonTestCase"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public async Task<ExpectedActual<TEntity>> GetSingleFromJsonStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                GetSingleFromStoredProcedure_ExpectedActual_Base(t, jsonTestCase, true, true));
        }


        private ExpectedActual<TEntity> GetSingleFromStoredProcedure_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync, bool isJsonColumn) {
            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var paramValues = jsonTestCase.GetObject<Dictionary<string, string>>("ParamValues", Output);

            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);

            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var queryStrings = new List<string> {
                $"spName={spName}"
            };
            foreach (var key in paramValues.Keys)
                queryStrings.Add($"{key}={paramValues[key]}");

            var url = $"{controllerPath}/{(isJsonColumn ? "json" : "sp")}/single{(isAsync ? "/async" : "")}?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            if (isAsync)
                actualResult = HttpClient.GetAsync<TEntity>(url).Result;
            else
                actualResult = HttpClient.Get<TEntity>(url);

            var statusCode = actualResult.GetStatusCode();

            TEntity actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");
            else
                actual = actualResult.GetObject<TEntity>();


            return new ExpectedActual<TEntity> { Expected = expected, Actual = actual };
        }




    }
}
