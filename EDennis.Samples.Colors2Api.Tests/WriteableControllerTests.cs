using System;
using System.Collections.Generic;
using System.Text;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.Colors2Api.Models;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
//using DevExtreme.AspNet.Data.ResponseModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using DevExtreme.AspNet.Data.ResponseModel;

namespace EDennis.Samples.Colors2Api.Tests {

    [Collection("Endpoint Tests")]
    public class WriteableControllerTests
        : ReadonlyEndpointTests<Startup> {


        public WriteableControllerTests(ITestOutputHelper output,
                ConfiguringWebApplicationFactory<Startup> factory)
            : base(output, factory) { }

        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Colors2", "EDennis.Samples.Colors2Api", "RgbController", methodName, testScenario, testCase) {
            }
        }

        [Theory]
        [TestJson_("GetDevExtreme", "FilterSkipTake", "RedGt200Skip2Take5")]
        public void GetDevExtreme1(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            //var filter = jsonTestCase.JsonTestFiles.Where(x => x.TestFile == "Filter").FirstOrDefault().Json;//.  GetObject<string>("Filter");
            var filter = jsonTestCase.GetObject<string>("Filter");
            var skip = jsonTestCase.GetObject<int>("Skip");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase
                .GetObject<ICollection<dynamic>>("Expected")
                .OrderBy(x=>x.Id)
                .ToList();


            var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/rgb/devextreme/?filter={filter}&skip={skip}&take={take}")
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
                .OrderBy(x => x.Name)
                .ToList();

            var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/rgb/devextreme/?filter={filter}&select={select}&sort={sort}&take={take}")
                .GetObject<DeserializableLoadResult<dynamic>>();
            var actual = loadResult.data
                .OrderBy(x => x.Name)
                .ToList();

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("GetDynamicLinq", "WhereSkipTake", "RedGt200Skip2Take5")]
        public void GetDynamicLinq1(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var where = jsonTestCase.GetObject<string[]>("Where");
            var skip = jsonTestCase.GetObject<int>("Skip");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            var actual = HttpClient.Get<List<LoadResult>>($"api/rgb/linq/?where={where}&skip={skip}&take={take}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

        [Theory]
        [TestJson_("GetDynamicLinq", "WhereOrderBySelectTake", "NameContainsBlueSelectNameDescTake10")]
        public void GetDynamicLinq2(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);
            Output.WriteLine($"Db instance name: {InstanceName}");

            var where = jsonTestCase.GetObject<string[]>("Where");
            var select = jsonTestCase.GetObject<string[]>("Select");
            var orderBy = jsonTestCase.GetObject<string[]>("OrderBy");
            var take = jsonTestCase.GetObject<int>("Take");
            var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

            var actual = HttpClient.Get<List<LoadResult>>($"api/rgb/linq/?where={where}&select={select}&orderBy={orderBy}&take={take}").Value;

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }

    }
}
