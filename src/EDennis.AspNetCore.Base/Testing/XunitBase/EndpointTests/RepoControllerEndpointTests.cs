using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class RepoControllerEndpointTests<TEntity, TProgram, TLauncher>        
        : LauncherEndpointTests<TProgram, TLauncher>
        where TEntity : class, new()
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {
        public RepoControllerEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
        }

        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetById request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<TEntity>> GetById_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult getResult = HttpClient.Get<TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<TEntity>> {
                Expected = new EndpointTestResult<TEntity> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<TEntity> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<TEntity>();

            return eaResult;
        }

        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetByIdAsync request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<TEntity>>> GetByIdAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode", Output);
            var expected = jsonTestCase.GetObject<TEntity>("Expected");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult getResult = await HttpClient.GetAsync<TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<TEntity>> {
                Expected = new EndpointTestResult<TEntity> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<TEntity> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<TEntity>();

            return eaResult;
        }

        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a Delete request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<List<TEntity>>> Delete_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult deleteResult = HttpClient.Delete<TEntity>(url);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = deleteResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }


        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a DeleteAsync request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> DeleteAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult deleteResult = await HttpClient.DeleteAsync<TEntity>(url);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = deleteResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }


        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a Post request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<List<TEntity>>> Post_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}";
            Output.WriteLine($"url: {url}");

            IActionResult postResult = HttpClient.Post(url,input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = postResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a PostAsync request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> PostAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async";
            Output.WriteLine($"url: {url}");

            IActionResult postResult = await HttpClient.PostAsync(url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = postResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }




        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a Put request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<List<TEntity>>> Put_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult putResult = HttpClient.Put(url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = putResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a PutAsync request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> PutAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult putResult = await HttpClient.PostAsync(url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = putResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a Patch request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<List<TEntity>>> Patch_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic,TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult patchResult = HttpClientExtensions.Patch(HttpClient,url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = patchResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a PatchAsync request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>Id</term><description>Primary key</description>
        /// </item>
        /// <item>
        ///     <term>Input</term><description>JSON input object</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller</description>
        /// </item>
        /// <item>
        ///     <term>LinqWhere</term><description>Dynamic Linq expression for follow-up GET request</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> PatchAsync_ExpectedActual(
            string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var id = jsonTestCase.GetObject<TEntity>("Id", Output);
            var input = jsonTestCase.GetObject<dynamic,TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult patchResult = await HttpClientExtensions.PatchAsync(HttpClient, url, input);

            var getUrl = $"{controllerPath}/linq?where={linqWhere}&X-Testing-Reset";

            IActionResult getResult = HttpClient.Get<DynamicLinqResult<TEntity>>(getUrl);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = patchResult.GetStatusCode(),
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;

            return eaResult;
        }


        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetDevExtreme request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
        /// </item>
        /// <item>
        ///     <term>Select</term><description>Comma-delimited list of properties to return (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Filter</term><description>DevExtreme filter expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Sort</term><description>DevExtreme sort expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Skip</term><description>Number of records to skip (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Take</term><description>Page size (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<List<TEntity>>> GetDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");


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

            var actualLoadResult = HttpClient.Get<DeserializableLoadResult<TEntity>>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualLoadResult.GetObject<DeserializableLoadResult<TEntity>>().data.ToList();
            

            return eaResult;

        }


        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetDevExtremeAsync request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
        /// </item>
        /// <item>
        ///     <term>Select</term><description>Comma-delimited list of properties to return (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Filter</term><description>DevExtreme filter expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Sort</term><description>DevExtreme sort expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Skip</term><description>Number of records to skip (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Take</term><description>Page size (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> GetDevExtremeAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var filter = jsonTestCase.GetObjectOrDefault<string>("Filter", Output);
            var sort = jsonTestCase.GetObjectOrDefault<string>("Sort", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");


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

            var url = $"{controllerPath}/devextreme/async?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            var actualLoadResult = await HttpClient.GetAsync<DeserializableLoadResult<TEntity>>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<List<TEntity>>> {
                Expected = new EndpointTestResult<List<TEntity>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<TEntity>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualLoadResult.GetObject<DeserializableLoadResult<TEntity>>().data.ToList();


            return eaResult;

        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetDynamicLinq request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
        /// </item>
        /// <item>
        ///     <term>Where</term><description>Dynamic Linq Where expression(optional)</description>
        /// </item>
        /// <item>
        ///     <term>Select</term><description>Dynamic Linq Select expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>OrderBy</term><description>Dynamic Linq OrderBy expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Skip</term><description>Number of records to skip (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Take</term><description>Page size (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> GetDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>,TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

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

            var url = $"{controllerPath}/linq?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult = HttpClient.Get<DynamicLinqResult<TEntity>,TEntity>(url);


            var eaResult = new ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualResult.GetObject<DynamicLinqResult<dynamic>>();

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetDynamicLinq request.</para>
        /// <para>----------------------------------</para>
        /// <para>REQUIRED/OPTIONAL TESTJSON FILES</para>
        /// <para>----------------------------------</para>
        /// <list type="table">
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
        /// </item>
        /// <item>
        ///     <term>Where</term><description>Dynamic Linq Where expression(optional)</description>
        /// </item>
        /// <item>
        ///     <term>Select</term><description>Dynamic Linq Select expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>OrderBy</term><description>Dynamic Linq OrderBy expression (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Skip</term><description>Number of records to skip (optional)</description>
        /// </item>
        /// <item>
        ///     <term>Take</term><description>Page size (optional)</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>>> GetDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            var where = jsonTestCase.GetObjectOrDefault<string>("Where", Output);
            var orderBy = jsonTestCase.GetObjectOrDefault<string>("OrderBy", Output);
            var select = jsonTestCase.GetObjectOrDefault<string>("Select", Output);
            var skip = jsonTestCase.GetObjectOrDefault<int?>("Skip", Output);
            var take = jsonTestCase.GetObjectOrDefault<int?>("Take", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<DynamicLinqResult<dynamic>, TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

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

            var url = $"{controllerPath}/linq?{string.Join('&', queryStrings)}";

            Output.WriteLine($"url: {url}");

            IActionResult actualResult = await HttpClient.GetAsync<DynamicLinqResult<TEntity>, TEntity>(url);


            var eaResult = new ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = 200,
                }
            };
            if (eaResult.Actual.StatusCode < 300 && eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualResult.GetObject<DynamicLinqResult<dynamic>>();

            return eaResult;
        }


    }
}