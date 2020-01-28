using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Text.Json;
using System.Diagnostics;

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
        /// for a Get request.</para>
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
        public ExpectedActual<EndpointTestResult<TEntity>> Get_ExpectedActual(
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
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<TEntity>();

            return eaResult;
        }

        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetAsync request.</para>
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
        public async Task<ExpectedActual<EndpointTestResult<TEntity>>> GetAsync_ExpectedActual(
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
            if (eaResult.Expected.Data != null)
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
            if (eaResult.Expected.Data != null)
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
            if (eaResult.Expected.Data != null)
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
            if (eaResult.Expected.Data != null)
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
            if (eaResult.Expected.Data != null)
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

            var id = jsonTestCase.GetObject<string>("Id", Output);
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
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<DynamicLinqResult<TEntity>>().Data;
            Debug.WriteLine("EXPECTED");
            Debug.WriteLine(JsonSerializer.Serialize(eaResult.Expected.Data, new JsonSerializerOptions { WriteIndented = true }));
            Debug.WriteLine("ACTUAL");
            Debug.WriteLine(JsonSerializer.Serialize(eaResult.Actual.Data, new JsonSerializerOptions { WriteIndented = true }));
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

            var id = jsonTestCase.GetObject<string>("Id", Output);
            var input = jsonTestCase.GetObject<TEntity>("Input", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var linqWhere = jsonTestCase.GetObject<string>("LinqWhere", Output);
            var expected = jsonTestCase.GetObject<List<TEntity>>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/async/{id}";
            Output.WriteLine($"url: {url}");

            IActionResult putResult = await HttpClient.PutAsync(url, input);

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
            if (eaResult.Expected.Data != null)
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

            var id = jsonTestCase.GetObject<string>("Id", Output);
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
            if (eaResult.Expected.Data != null)
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

            var id = jsonTestCase.GetObject<string>("Id", Output);
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
            if (eaResult.Expected.Data != null)
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
        public ExpectedActual<EndpointTestResult<List<TEntity>>> GetWithDevExtreme_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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
                    StatusCode = actualLoadResult.GetStatusCode()
                }
            };
            if (eaResult.Expected.Data != null)
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
        public async Task<ExpectedActual<EndpointTestResult<List<TEntity>>>> GetWithDevExtremeAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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
                    StatusCode = actualLoadResult.GetStatusCode()
                }
            };
            if (eaResult.Expected.Data != null)
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
        public ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> GetWithDynamicLinq_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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

            IActionResult actualResult = HttpClient.Get<DynamicLinqResult<dynamic>,TEntity>(url);


            var eaResult = new ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = actualResult.GetStatusCode()
                }
            };
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualResult.GetObject<DynamicLinqResult<dynamic>>();

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetDynamicLinqAsync request.</para>
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
        public async Task<ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>>> GetWithDynamicLinqAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {
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

            IActionResult actualResult = await HttpClient.GetAsync<DynamicLinqResult<dynamic>, TEntity>(url);


            var eaResult = new ExpectedActual<EndpointTestResult<DynamicLinqResult<dynamic>>> {
                Expected = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<DynamicLinqResult<dynamic>> {
                    StatusCode = actualResult.GetStatusCode()
                }
            };
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = actualResult.GetObject<DynamicLinqResult<dynamic>>();

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonObjectFromStoredProcedure request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/{spName}?{param1}={value1}&{param2}={value2}</para>
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
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<dynamic>> GetJsonObjectFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string,string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<dynamic, TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if(qry.Count > 0)
                url = url + "?" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            IActionResult getResult = HttpClient.Get<dynamic, TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<dynamic>> {
                Expected = new EndpointTestResult<dynamic> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<dynamic> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<dynamic>();

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonObjectFromStoredProcedureAsync request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/{spName}?{param1}={value1}&{param2}={value2}</para>
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
        ///     <term>SpName</term><description>The name of the stored procedure -- also the relative path of the action method</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<dynamic>>> GetJsonObjectFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<dynamic, TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "?" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            IActionResult getResult = await HttpClient.GetAsync<dynamic, TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<dynamic>> {
                Expected = new EndpointTestResult<dynamic> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<dynamic> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<dynamic>();

            return eaResult;
        }




        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonArrayFromStoredProcedure request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/{spName}?{param1}={value1}&{param2}={value2}</para>
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
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public ExpectedActual<EndpointTestResult<List<dynamic>>> GetJsonArrayFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<List<dynamic>, TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "?" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            IActionResult getResult = HttpClient.Get<List<dynamic>, TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<List<dynamic>>> {
                Expected = new EndpointTestResult<List<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<dynamic>> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<List<dynamic>>();

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonArrayFromStoredProcedureAsync request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/{spName}?{param1}={value1}&{param2}={value2}</para>
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
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="t">TestScenario(TestCase)</param>
        /// <param name="jsonTestCase">Test input parameters and expected results</param>
        /// <returns>An object holding expected and actual status code and response body (data)</returns>
        public async Task<ExpectedActual<EndpointTestResult<List<dynamic>>>> GetJsonArrayFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<List<dynamic>, TEntity>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/{spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "?" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            IActionResult getResult = await HttpClient.GetAsync<List<dynamic>, TEntity>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<List<dynamic>>> {
                Expected = new EndpointTestResult<List<dynamic>> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<List<dynamic>> {
                    StatusCode = getResult.GetStatusCode(),
                }
            };
            if (eaResult.Expected.Data != null)
                eaResult.Actual.Data = getResult.GetObject<List<dynamic>>();

            return eaResult;
        }


    }
}