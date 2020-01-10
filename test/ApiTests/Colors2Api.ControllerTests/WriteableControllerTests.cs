using Colors2.Models;
using Colors2Api.Lib.Controllers;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Colors2Api.ControllerTests {

    [Collection("Controller Tests")]
    public class WriteableControllerTests : SqlServerWriteableControllerTests<RgbController,RgbRepo,Rgb,Color2DbContext>
        {
        public WriteableControllerTests(ITestOutputHelper output) : base(output) {
        }


        internal class TestJson_ : TestJsonAttribute {
            public TestJson_(string methodName, string testScenario, string testCase)
                : base("Color2Db", "Colors2Api", "RgbController", methodName, testScenario, testCase) {
            }
        }


        //NOTE: OData is endpoint testable only

        [Theory]
        [TestJson_("GetDevExtreme", "FilterSkipTake", "A")]
        [TestJson_("GetDevExtreme", "FilterSortSelectTake", "B")]
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            Output.WriteLine(t);

            var select = jsonTestCase.TestScenario.Contains("Select") ? jsonTestCase.GetObject<string>("Select") : default;
            var filter = jsonTestCase.GetObject<string>("Filter");
            var sort = jsonTestCase.TestScenario.Contains("Sort") ? jsonTestCase.GetObject<string>("Sort") : default;
            var skip = jsonTestCase.TestScenario.Contains("Skip") ? jsonTestCase.GetObject<int>("Skip") : default;
            var take = jsonTestCase.GetObject<int>("Take");
            var expectedDynamic = jsonTestCase.GetObject<List<dynamic>>("Expected");

            var expected = ObjectExtensions.ToPropertyDictionaryList(expectedDynamic);

            var actualDynamic = GetDevExtremeResult(select, sort, filter, skip, take, null, null, null);
            var actual = ObjectExtensions.ToPropertyDictionaryList(actualDynamic);

            Assert.True(actual.IsEqualOrWrite(expected, Output));
        }


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

        //    var loadResult = HttpClient.Get<DeserializableLoadResult<dynamic>>($"api/rgb/devextreme?filter={filter}&select={select}&sort={sort}&take={take}")
        //        .GetObject<DeserializableLoadResult<dynamic>>();
        //    var actual = loadResult.data
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("GetDynamicLinq", "WhereSkipTake", "RedGt200Skip2Take5")]
        //public void GetDynamicLinq1(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var where = jsonTestCase.GetObject<string>("Where");
        //    var skip = jsonTestCase.GetObject<int>("Skip");
        //    var take = jsonTestCase.GetObject<int>("Take");
        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/rgb/linq?where={where}&skip={skip}&take={take}")
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

        //    var actual = HttpClient.Get<List<dynamic>>($"api/rgb/linq?where={where}&select={select}&orderBy={orderBy}&take={take}")
        //        .GetObject<List<dynamic>>()
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));

        //}


        //[Theory]
        //[TestJson_("GetDynamicLinqAsync", "WhereSkipTake", "RedGt200Skip2Take5")]
        //public void GetDynamicLinqAsync1(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var where = jsonTestCase.GetObject<string>("Where");
        //    var skip = jsonTestCase.GetObject<int>("Skip");
        //    var take = jsonTestCase.GetObject<int>("Take");
        //    var expected = jsonTestCase.GetObject<List<dynamic>>("Expected")
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    var actual = HttpClient.Get<List<dynamic>>($"api/rgb/linq/async?where={where}&skip={skip}&take={take}")
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

        //    var actual = HttpClient.Get<List<dynamic>>($"api/rgb/linq/async?where={where}&select={select}&orderBy={orderBy}&take={take}")
        //        .GetObject<List<dynamic>>()
        //        .ToList();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));

        //}


        //[Theory]
        //[TestJson_("Get", "", "1")]
        //[TestJson_("Get", "", "2")]
        //public void Get(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var expected = jsonTestCase.GetObject<Rgb>("Expected");

        //    var actual = HttpClient.Get<Rgb>($"api/rgb/{id}")
        //        .GetObject<Rgb>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("GetAsync", "", "1")]
        //[TestJson_("GetAsync", "", "2")]
        //public void GetAsync(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var expected = jsonTestCase.GetObject<Rgb>("Expected");

        //    var actual = HttpClient.Get<Rgb>($"api/rgb/async/{id}")
        //        .GetObject<Rgb>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}


        //[Theory]
        //[TestJson_("Delete", "", "1")]
        //[TestJson_("Delete", "", "2")]
        //public void Delete(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

        //    HttpClient.Delete<Rgb>($"api/rgb/{id}");

        //    var actual = HttpClient.Get<List<Rgb>>($"api/rgb/linq")
        //        .GetObject<List<Rgb>>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("DeleteAsync", "", "1")]
        //[TestJson_("DeleteAsync", "", "2")]
        //public void DeleteAsync(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

        //    HttpClient.Delete<Rgb>($"api/rgb/async/{id}");

        //    var actual = HttpClient.Get<List<Rgb>>($"api/rgb/linq")
        //        .GetObject<List<Rgb>>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("Put", "", "1")]
        //[TestJson_("Put", "", "2")]
        //public void Put(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var input = jsonTestCase.GetObject<Rgb>("Input");
        //    var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

        //    HttpClient.Put($"api/rgb/{id}",input);

        //    var actual = HttpClient.Get<List<Rgb>>($"api/rgb/linq")
        //        .GetObject<List<Rgb>>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("PutAsync", "", "1")]
        //[TestJson_("PutAsync", "", "2")]
        //public void PutAsync(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var id = jsonTestCase.GetObject<int>("Id");
        //    var input = jsonTestCase.GetObject<Rgb>("Input");
        //    var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

        //    HttpClient.Put($"api/rgb/async/{id}", input);

        //    var actual = HttpClient.Get<List<Rgb>>($"api/rgb/linq")
        //        .GetObject<List<Rgb>>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}


        //[Theory]
        //[TestJson_("Post", "", "1")]
        //[TestJson_("Post", "", "2")]
        //public void Post(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var input = jsonTestCase.GetObject<Rgb>("Input");
        //    var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

        //    HttpClient.Post($"api/rgb", input);

        //    var actual = HttpClient.Get<List<Rgb>>($"api/rgb/linq")
        //        .GetObject<List<Rgb>>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

        //[Theory]
        //[TestJson_("PostAsync", "", "1")]
        //[TestJson_("PostAsync", "", "2")]
        //public void PostAsync(string t, JsonTestCase jsonTestCase) {
        //    Output.WriteLine(t);
        //    Output.WriteLine($"Db instance name: {InstanceName}");

        //    var input = jsonTestCase.GetObject<Rgb>("Input");
        //    var expected = jsonTestCase.GetObject<List<Rgb>>("Expected");

        //    HttpClient.Post($"api/rgb/async", input);

        //    var actual = HttpClient.Get<List<Rgb>>($"api/rgb/linq")
        //        .GetObject<List<Rgb>>();

        //    Assert.True(actual.IsEqualOrWrite(expected, Output));
        //}

    }
}
