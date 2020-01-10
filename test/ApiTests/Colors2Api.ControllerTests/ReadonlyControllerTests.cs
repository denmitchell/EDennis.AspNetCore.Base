using Colors2.Models;
using Colors2Api.Lib.Controllers;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Colors2Api.Tests
{

    [Collection("Controller Tests")]
    public class ReadonlyControllerTests : SqlServerReadonlyControllerTests<HslController, HslRepo, Hsl, Color2DbContext> {
        public ReadonlyControllerTests(ITestOutputHelper output) : base(output) {
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Colors2Db", "Colors2Api", "HslController", methodName, testScenario, testCase) {
            }
        }




        //[Theory]
        //[TestJson_("GetOData", "FilterSkipTop", "HueGt200Skip2Top5")]
        //public void GetOData1(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var filter = jsonTestCase.GetObject<string>("Filter");
        //    var skip = jsonTestCase.GetObject<int>("Skip");
        //    var top = jsonTestCase.GetObject<int>("Top");
        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/hsl/odata?$filter={filter}&$skip={skip}&$top={top}")
        //        .GetObject<List<dynamic>>()
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("GetOData", "FilterOrderBySelectTop", "NameContainsBlueSelectNameDescSysUserTop10")]
        //public void GetOData2(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var filter = jsonTestCase.GetObject<string>("Filter");
        //    var select = jsonTestCase.GetObject<string>("Select");
        //    var orderBy = jsonTestCase.GetObject<string>("OrderBy");
        //    var top = jsonTestCase.GetObject<int>("Top");

        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderByDescending(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/hsl/odata?$filter={filter}&$select={select}&$orderBy={orderBy}&$top={top}")
        //        .GetObject<List<dynamic>>()
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));

        //}


        //[Theory]
        //[TestJson_("GetDevExtreme", "FilterSkipTake", "HueGt200Skip2Take5")]
        //public void GetDevExtreme1(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var filter = jsonTestCase.GetObject<string>("Filter");
        //    var skip = jsonTestCase.GetObject<int>("Skip");
        //    var take = jsonTestCase.GetObject<int>("Take");
        //    var expected = jsonTestCase
        //        .GetObject<ICollection<dynamic>>("Expected")
        //        .OrderBy(x => x.Id)
        //        .ToList();


        //    var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/hsl/devextreme?filter={filter}&skip={skip}&take={take}")
        //        .GetObject<DeserializableLoadResult<dynamic>>();
        //    var actual = loadResult.data
        //        .OrderBy(x => x.Id)
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}


        //[Theory]
        //[TestJson_("GetDevExtreme", "FilterSortSelectTake", "NameContainsBlueSelectNameDescSysUserTake10")]
        //public void GetDevExtreme2(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var filter = jsonTestCase.GetObject<string>("Filter");
        //    var select = jsonTestCase.GetObject<string>("Select");
        //    var sort = jsonTestCase.GetObject<string>("Sort");
        //    var take = jsonTestCase.GetObject<int>("Take");
        //    var expected = jsonTestCase
        //        .GetObject<ICollection<dynamic>>("Expected")
        //        .OrderByDescending(x => x.Name)
        //        .ToList();

        //    var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/hsl/devextreme?filter={filter}&select={select}&sort={sort}&take={take}")
        //        .GetObject<DeserializableLoadResult<dynamic>>();
        //    var actual = loadResult.data
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("GetDynamicLinq", "WhereSkipTake", "HueGt200Skip2Take5")]
        //public void GetDynamicLinq1(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var where = jsonTestCase.GetObject<string>("Where");
        //    var skip = jsonTestCase.GetObject<int>("Skip");
        //    var take = jsonTestCase.GetObject<int>("Take");
        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq?where={where}&skip={skip}&take={take}")
        //        .GetObject<List<dynamic>>()
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("GetDynamicLinq", "WhereOrderBySelectTake", "NameContainsBlueSelectNameDescSysUserTake10")]
        //public void GetDynamicLinq2(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var where = jsonTestCase.GetObject<string>("Where");
        //    var select = jsonTestCase.GetObject<string>("Select");
        //    var orderBy = jsonTestCase.GetObject<string>("OrderBy");
        //    var take = jsonTestCase.GetObject<int>("Take");

        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderByDescending(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq?where={where}&select={select}&orderBy={orderBy}&take={take}")
        //        .GetObject<List<dynamic>>()
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));

        //}


        //[Theory]
        //[TestJson_("GetDynamicLinqAsync", "WhereSkipTake", "HueGt200Skip2Take5")]
        //public void GetDynamicLinqAsync1(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var where = jsonTestCase.GetObject<string>("Where");
        //    var skip = jsonTestCase.GetObject<int>("Skip");
        //    var take = jsonTestCase.GetObject<int>("Take");
        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq/async?where={where}&skip={skip}&take={take}")
        //        .GetObject<List<dynamic>>()
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("GetDynamicLinqAsync", "WhereOrderBySelectTake", "NameContainsBlueSelectNameDescSysUserTake10")]
        //public void GetDynamicLinqAsync2(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var where = jsonTestCase.GetObject<string>("Where");
        //    var select = jsonTestCase.GetObject<string>("Select");
        //    var orderBy = jsonTestCase.GetObject<string>("OrderBy");
        //    var take = jsonTestCase.GetObject<int>("Take");

        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderByDescending(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq/async?where={where}&select={select}&orderBy={orderBy}&take={take}")
        //        .GetObject<List<dynamic>>()
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));

        //}


        //[Theory]
        //[TestJson_("GetFromStoredProcedure", "HslByColorName", "AliceBlue")]
        //[TestJson_("GetFromStoredProcedure", "HslByColorName", "DarkKhaki")]
        //public void GetFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var spName = jsonTestCase.GetObject<string>("SpName");
        //    var colorName = jsonTestCase.GetObject<string>("ColorName");

        //    dynamic expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .FirstOrDefault();

        //    dynamic actual = HttpClient.Get<List<dynamic>>($"api/hsl/sp?spName={spName}&colorName={colorName}")
        //        .GetObject<List<dynamic>>()
        //        .FirstOrDefault();

        //    Assert.Equal(expected,actual);
        //}


        //[Theory]
        //[TestJson_("GetFromStoredProcedureAsync", "HslByColorName", "AliceBlue")]
        //[TestJson_("GetFromStoredProcedureAsync", "HslByColorName", "DarkKhaki")]
        //public void GetFromStoredProcedureAsync(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var spName = jsonTestCase.GetObject<string>("SpName");
        //    var colorName = jsonTestCase.GetObject<string>("ColorName");

        //    dynamic expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .FirstOrDefault();

        //    dynamic actual = HttpClient.Get<List<dynamic>>($"api/hsl/sp/async?spName={spName}&colorName={colorName}")
        //        .GetObject<List<dynamic>>()
        //        .FirstOrDefault();

        //    Assert.Equal(expected, actual);
        //}

        //[Theory]
        //[TestJson_("GetJsonColumnFromStoredProcedure", "RgbJsonByColorName", "AliceBlue")]
        //[TestJson_("GetJsonColumnFromStoredProcedure", "RgbJsonByColorName", "DarkKhaki")]
        //public void GetJsonColumnFromStoredProcedure(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var spName = jsonTestCase.GetObject<string>("SpName");
        //    var colorName = jsonTestCase.GetObject<string>("ColorName");

        //    dynamic expected =
        //        JToken.Parse(
        //         JToken.Parse(jsonTestCase.GetObject<string>("Expected"))
        //            .SelectToken("Json")
        //            .ToString())
        //        .ToObject<dynamic>();


        //    var expectedRgb = new {
        //        expected.Red,
        //        expected.Green,
        //        expected.Blue
        //    };

        //    dynamic actual = JToken.Parse(HttpClient.Get<string>($"api/hsl/json?spName={spName}&colorName={colorName}")
        //        .GetObject<string>())
        //        .ToObject<dynamic>();

        //    var actualRgb = new {
        //        actual.Red,
        //        actual.Green,
        //        actual.Blue
        //    };

        //    Assert.Equal(expectedRgb,actualRgb);
        //}


    }
}
