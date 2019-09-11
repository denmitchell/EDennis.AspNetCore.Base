using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.Colors2Api.Tests
{

    [Collection("Endpoint Tests")]
    public class ReadonlyControllerTests
        : ReadonlyEndpointTests<Startup> {


        public ReadonlyControllerTests(ITestOutputHelper output,
                ConfiguringWebApplicationFactory<Startup> factory)
            : base(output, factory) { }

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Colors2", "EDennis.Samples.Colors2Api", "HslController", methodName, testScenario, testCase) {
            }
        }



        [Theory]
        [TestJson_("GetOData", "FilterSkipTop", "HueGt200Skip2Top5")]
        public void GetOData1(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var filter = jsonTestCase.GetObject<string>("Filter");
            var skip = jsonTestCase.GetObject<int>("Skip");
            var top = jsonTestCase.GetObject<int>("Top");
            var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .OrderBy(x => x.Name)
                .ToList();

            var actual = HttpClient.Get<List<dynamic>>($"api/hsl/odata?$filter={filter}&$skip={skip}&$top={top}")
                .GetObject<List<dynamic>>()
                .OrderBy(x => x.Name)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("GetOData", "FilterOrderBySelectTop", "NameContainsBlueSelectNameDescSysUserTop10")]
        public void GetOData2(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var filter = jsonTestCase.GetObject<string>("Filter");
            var select = jsonTestCase.GetObject<string>("Select");
            var orderBy = jsonTestCase.GetObject<string>("OrderBy");
            var top = jsonTestCase.GetObject<int>("Top");

            var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .OrderByDescending(x => x.Name)
                .ToList();

            var actual = HttpClient.Get<List<dynamic>>($"api/hsl/odata?$filter={filter}&$select={select}&$orderBy={orderBy}&$top={top}")
                .GetObject<List<dynamic>>()
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));

        }


        [Theory]
        [TestJson_("GetDevExtreme", "FilterSkipTake", "HueGt200Skip2Take5")]
        public void GetDevExtreme1(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var filter = jsonTestCase.GetObject<string>("Filter");
            var skip = jsonTestCase.GetObject<int>("Skip");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase
                .GetObject<ICollection<dynamic>>("Expected")
                .OrderBy(x=>x.Id)
                .ToList();


            var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/hsl/devextreme?filter={filter}&skip={skip}&take={take}")
                .GetObject<DeserializableLoadResult<dynamic>>();
            var actual = loadResult.data
                .OrderBy(x => x.Id)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }


        [Theory]
        [TestJson_("GetDevExtreme", "FilterSortSelectTake", "NameContainsBlueSelectNameDescSysUserTake10")]
        public void GetDevExtreme2(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var filter = jsonTestCase.GetObject<string>("Filter");
            var select = jsonTestCase.GetObject<string>("Select");
            var sort = jsonTestCase.GetObject<string>("Sort");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase
                .GetObject<ICollection<dynamic>>("Expected")
                .OrderByDescending(x => x.Name)
                .ToList();

            var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/hsl/devextreme?filter={filter}&select={select}&sort={sort}&take={take}")
                .GetObject<DeserializableLoadResult<dynamic>>();
            var actual = loadResult.data
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("GetDynamicLinq", "WhereSkipTake", "HueGt200Skip2Take5")]
        public void GetDynamicLinq1(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var where = jsonTestCase.GetObject<string>("Where");
            var skip = jsonTestCase.GetObject<int>("Skip");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .OrderBy(x => x.Name)
                .ToList();

            var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq?where={where}&skip={skip}&take={take}")
                .GetObject<List<dynamic>>()
                .OrderBy(x => x.Name)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("GetDynamicLinq", "WhereOrderBySelectTake", "NameContainsBlueSelectNameDescSysUserTake10")]
        public void GetDynamicLinq2(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var where = jsonTestCase.GetObject<string>("Where");
            var select = jsonTestCase.GetObject<string>("Select");
            var orderBy = jsonTestCase.GetObject<string>("OrderBy");
            var take = jsonTestCase.GetObject<int>("Take");

            var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .OrderByDescending(x => x.Name)
                .ToList();

            var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq?where={where}&select={select}&orderBy={orderBy}&take={take}")
                .GetObject<List<dynamic>>()
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));

        }


        [Theory]
        [TestJson_("GetDynamicLinqAsync", "WhereSkipTake", "HueGt200Skip2Take5")]
        public void GetDynamicLinqAsync1(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var where = jsonTestCase.GetObject<string>("Where");
            var skip = jsonTestCase.GetObject<int>("Skip");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .OrderBy(x => x.Name)
                .ToList();

            var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq/async?where={where}&skip={skip}&take={take}")
                .GetObject<List<dynamic>>()
                .OrderBy(x => x.Name)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("GetDynamicLinqAsync", "WhereOrderBySelectTake", "NameContainsBlueSelectNameDescSysUserTake10")]
        public void GetDynamicLinqAsync2(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var where = jsonTestCase.GetObject<string>("Where");
            var select = jsonTestCase.GetObject<string>("Select");
            var orderBy = jsonTestCase.GetObject<string>("OrderBy");
            var take = jsonTestCase.GetObject<int>("Take");

            var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
                .OrderByDescending(x => x.Name)
                .ToList();

            var actual = HttpClient.Get<List<dynamic>>($"api/hsl/linq/async?where={where}&select={select}&orderBy={orderBy}&take={take}")
                .GetObject<List<dynamic>>()
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));

        }


    }
}
