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
using Microsoft.EntityFrameworkCore;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SqlServerControllerEndpointTests<TContext, TProgram, TLauncher>        
        : LauncherEndpointTests<TProgram, TLauncher>
        where TContext : DbContext, ISqlServerDbContext<TContext>
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {
        public SqlServerControllerEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
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
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// </list>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
        /// </item>
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
        ///     <term>SpName</term><description>The name of the stored procedure -- also the relative path of the action method</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
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
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
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
        ///     <term>SpName</term><description>The name of the stored procedure</description>
        /// </item>
        /// <item>
        ///     <term>Params</term><description>A JSON object holding the stored procedure parameters</description>
        /// </item>
        /// <item>
        ///     <term>ControllerPath</term><description>Relative path to the controller (required)</description>
        /// </item>
        /// <item>
        ///     <term>ExpectedStatusCode</term><description>Expected HTTP status code (required)</description>
        /// </item>
        /// <item>
        ///     <term>Expected</term><description>Expected response body (required)</description>
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