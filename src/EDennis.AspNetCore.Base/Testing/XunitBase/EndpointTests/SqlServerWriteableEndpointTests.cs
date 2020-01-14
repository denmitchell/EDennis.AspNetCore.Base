using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerWriteableEndpointTests<TEntity, TProgram, TLauncher>
        : SqlServerReadonlyEndpointTests<TEntity, TProgram, TLauncher>
        where TEntity : class, new()
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {

        public SqlServerWriteableEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
        }

        public async Task<ExpectedActual<TEntity>> GetAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                Get_ExpectedActual_Base(t, jsonTestCase, true));
        }

        public ExpectedActual<TEntity> Get_ExpectedActual(string t, JsonTestCase jsonTestCase)
            => Get_ExpectedActual_Base(t, jsonTestCase, false);

        public ExpectedActual<TEntity> Get_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var url = $"{controllerPath}/{(isAsync ? "async/" : "")}{id}";
            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            if(isAsync)
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



        public async Task<ExpectedActualList<TEntity>> DeleteAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                Delete_ExpectedActual_Base(t, jsonTestCase, true));
        }

        public ExpectedActualList<TEntity> Delete_ExpectedActual(string t, JsonTestCase jsonTestCase)
            => Delete_ExpectedActual_Base(t, jsonTestCase, false);

        public ExpectedActualList<TEntity> Delete_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var start = jsonTestCase.GetObject<string>("WindowStart", Output);
            var end = jsonTestCase.GetObject<string>("WindowEnd", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var url = $"{controllerPath}/{(isAsync ? "async/" : "")}{id}";
            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            if (isAsync)
                HttpClient.DeleteAsync<TEntity>(url).Wait();
            else
                HttpClient.Delete<TEntity>(url);

            var getUrl = $"{controllerPath}?where Id ge {start} and Id le {end}";

            actualResult = HttpClient.Get<List<TEntity>>(getUrl);
            var statusCode = actualResult.GetStatusCode();

            List<TEntity> actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");
            else
                actual = actualResult.GetObject<List<TEntity>>();

            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
        }



        public async Task<ExpectedActualList<TEntity>> PutAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                Put_ExpectedActual_Base(t, jsonTestCase, true));
        }

        public ExpectedActualList<TEntity> Put_ExpectedActual(string t, JsonTestCase jsonTestCase)
            => Put_ExpectedActual_Base(t, jsonTestCase, false);

        public ExpectedActualList<TEntity> Put_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<TEntity>("Input");
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var start = jsonTestCase.GetObject<string>("WindowStart", Output);
            var end = jsonTestCase.GetObject<string>("WindowEnd", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var url = $"{controllerPath}/{(isAsync ? "async/" : "")}{id}";
            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            if (isAsync)
                HttpClient.PutAsync(url,input).Wait();
            else
                HttpClient.Put(url,input);

            var getUrl = $"{controllerPath}?where Id ge {start} and Id le {end}";

            actualResult = HttpClient.Get<List<TEntity>>(getUrl);
            var statusCode = actualResult.GetStatusCode();

            List<TEntity> actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");
            else
                actual = actualResult.GetObject<List<TEntity>>();

            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
        }



        public async Task<ExpectedActualList<TEntity>> PostAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            return await Task.Run(() =>
                Post_ExpectedActual_Base(t, jsonTestCase, true));
        }

        public ExpectedActualList<TEntity> Post_ExpectedActual(string t, JsonTestCase jsonTestCase)
            => Post_ExpectedActual_Base(t, jsonTestCase, false);

        public ExpectedActualList<TEntity> Post_ExpectedActual_Base(string t, JsonTestCase jsonTestCase, bool isAsync) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<int>("Id");
            var input = jsonTestCase.GetObject<TEntity>("Input");
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var start = jsonTestCase.GetObject<string>("WindowStart", Output);
            var end = jsonTestCase.GetObject<string>("WindowEnd", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");

            var url = $"{controllerPath}/{(isAsync ? "async/" : "")}";
            Output.WriteLine($"url: {url}");

            IActionResult actualResult;
            if (isAsync)
                HttpClient.PostAsync(url, input).Wait();
            else
                HttpClient.Post(url, input);

            var getUrl = $"{controllerPath}?where (Id ge {start} and Id le {end}) or Id eq {id}";

            actualResult = HttpClient.Get<List<TEntity>>(getUrl);
            var statusCode = actualResult.GetStatusCode();

            List<TEntity> actual;
            if (statusCode > 299)
                throw new Exception($"StatusCode: {statusCode}\n Text:{actualResult.GetObject<string>()}");
            else
                actual = actualResult.GetObject<List<TEntity>>();

            return new ExpectedActualList<TEntity> { Expected = expected, Actual = actual };
        }



    }
}
