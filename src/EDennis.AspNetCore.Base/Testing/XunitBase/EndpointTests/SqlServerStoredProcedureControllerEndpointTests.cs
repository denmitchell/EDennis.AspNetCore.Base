using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing
{
    public abstract class SqlServerStoredProcedureControllerEndpointTests<TContext, TProgram, TLauncher>        
        : LauncherEndpointTests<TProgram, TLauncher>
        where TContext : DbContext, ISqlServerDbContext<TContext>
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {
        public SqlServerStoredProcedureControllerEndpointTests(ITestOutputHelper output, LauncherFixture<TProgram, TLauncher> launcherFixture) : base(output, launcherFixture) {
        }

        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonObjectFromStoredProcedure request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/object?spName={spName}&{param1}={value1}[&...]</para>
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
        public ExpectedActual<string> GetJsonObjectFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string,string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<string>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/object?spName={spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if(qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<string> getResult = HttpClient.Get<string>(url);
            return new ExpectedActual<string>
            {
                Expected = BuildEndpointTestResultJson(expectedStatusCode, expected),
                Actual = BuildEndpointTestResultJson(getResult.StatusCode.Value, getResult.TypedValue)
            };

        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonObjectFromStoredProcedureAsync request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/object/async?spName={spName}&{param1}={value1}[&...]</para>
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
        public async Task<ExpectedActual<string>> GetJsonObjectFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<string>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/object/async?spName={spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");


            ObjectResult<string> getResult = await HttpClient.GetAsync<string>(url);
            return new ExpectedActual<string>
            {
                Expected = BuildEndpointTestResultJson(expectedStatusCode, expected),
                Actual = BuildEndpointTestResultJson(getResult.StatusCode.Value, getResult.TypedValue)
            };

        }




        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonArrayFromStoredProcedure request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/array?spName={spName}&{param1}={value1}[&...]</para>
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
        public ExpectedActual<string> GetJsonArrayFromStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<string>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/array?spName={spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<string> getResult = HttpClient.Get<string>(url);
            return new ExpectedActual<string>
            {
                Expected = BuildEndpointTestResultJson(expectedStatusCode, expected),
                Actual = BuildEndpointTestResultJson(getResult.StatusCode.Value, getResult.TypedValue)
            };

        }





        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonArrayFromStoredProcedureAsync request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/array/async?spName={spName}&{param1}={value1}[&...]</para>
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
        public async Task<ExpectedActual<string>> GetJsonArrayFromStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<string>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/array/async?spName={spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<string> getResult = await HttpClient.GetAsync<string>(url);
            return new ExpectedActual<string>
            {
                Expected = BuildEndpointTestResultJson(expectedStatusCode, expected),
                Actual = BuildEndpointTestResultJson(getResult.StatusCode.Value, getResult.TypedValue)
            };

        }




        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonFromJsonStoredProcedure request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/forjson?spName={spName}&{param1}={value1}[&...]</para>
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
        public ExpectedActual<string> GetJsonFromJsonStoredProcedure_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<string>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/forjson?spName={spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<string> getResult = HttpClient.Get<string>(url);
            return new ExpectedActual<string>
            {
                Expected = BuildEndpointTestResultJson(expectedStatusCode, expected),
                Actual = BuildEndpointTestResultJson(getResult.StatusCode.Value, getResult.TypedValue)
            };
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetJsonFromJsonStoredProcedureAsync request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/forjson/async?spName={spName}&{param1}={value1}[&...]</para>
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
        public async Task<ExpectedActual<string>> GetJsonFromJsonStoredProcedureAsync_ExpectedActual(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<string>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/forjson/async?spName={spName}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<string> getResult = await HttpClient.GetAsync<string>(url);
            return new ExpectedActual<string>
            {
                Expected = BuildEndpointTestResultJson(expectedStatusCode, expected),
                Actual = BuildEndpointTestResultJson(getResult.StatusCode.Value, getResult.TypedValue)
            };
        }




        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetScalarFromStoredProcedure request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/scalar?spName={spName}&{param1}={value1}[&...]</para>
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
        public ExpectedActual<EndpointTestResult<TResult>> GetScalarFromStoredProcedure_ExpectedActual<TResult>(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var returnType = jsonTestCase.GetObject<string>("ReturnType", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<TResult>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/scalar?spName={spName}&returnType={returnType}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<TResult> getResult = HttpClient.Get<TResult>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<TResult>> {
                Expected = new EndpointTestResult<TResult> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<TResult> {
                    StatusCode = getResult.GetStatusCode(),
                    Data = getResult.TypedValue
                }
            };

            return eaResult;
        }



        /// <summary>
        /// <para>Returns the expected and actual status code and response body (Data)
        /// for a GetScalarFromStoredProcedureAsync request.  Note that the 
        /// URL should conform to this pattern: 
        /// ...{controllerPath}/scalar/async?spName={spName}&{param1}={value1}[&...]</para>
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
        public async Task<ExpectedActual<EndpointTestResult<TResult>>> GetScalarFromStoredProcedureAsync_ExpectedActual<TResult>(string t, JsonTestCase jsonTestCase) {

            Output.WriteLine(t);

            var spName = jsonTestCase.GetObject<string>("SpName", Output);
            var returnType = jsonTestCase.GetObject<string>("ReturnType", Output);
            var parameters = jsonTestCase.GetObject<Dictionary<string, string>>("Params", Output);
            var controllerPath = jsonTestCase.GetObject<string>("ControllerPath", Output);
            var expected = jsonTestCase.GetObject<TResult>("Expected");
            var expectedStatusCode = jsonTestCase.GetObject<int>("ExpectedStatusCode");

            var url = $"{controllerPath}/scalar/async?spName={spName}&returnType={returnType}";

            var qry = new List<string>();
            foreach (var param in parameters)
                qry.Add($"{param.Key}={param.Value}");

            if (qry.Count > 0)
                url = url + "&" + string.Join('&', qry);

            Output.WriteLine($"url: {url}");

            ObjectResult<TResult> getResult = await HttpClient.GetAsync<TResult>(url);

            var eaResult = new ExpectedActual<EndpointTestResult<TResult>> {
                Expected = new EndpointTestResult<TResult> {
                    StatusCode = expectedStatusCode,
                    Data = expected
                },
                Actual = new EndpointTestResult<TResult> {
                    StatusCode = getResult.GetStatusCode(),
                    Data = getResult.TypedValue
                }
            };

            return eaResult;
        }

        /// <summary>
        /// Build a JSON object that encapsulates the statusCode and data
        /// as an EndpointTestResult
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string BuildEndpointTestResultJson(int statusCode, string data)
        {
            return $"{{\"StatusCode\":{statusCode},\"Data\":{data ?? "null"}}}";
        }



    }
}