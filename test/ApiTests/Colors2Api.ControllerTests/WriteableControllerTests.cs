using Colors2.Models;
using Colors2Api.Lib.Controllers;
using EDennis.AspNetCore.Base.Testing;
using EDennis.NetCoreTestingUtilities;
using EDennis.NetCoreTestingUtilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [TestJson_("GetDevExtreme", "WriteableControllerTests|FilterSkipTake", "A")]
        [TestJson_("GetDevExtreme", "WriteableControllerTests|FilterSortSelectTake", "B")]
        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereSkipTake", "A")] //RedGt200Skip2Take5
        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereOrderBySelectTake", "B")] //NameContainsBlueSelectNameDescSysUserTake10
        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }

        [Theory]
        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereSkipTake", "A")] //RedGt200Skip2Take5
        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereOrderBySelectTake", "B")] //NameContainsBlueSelectNameDescSysUserTake10
        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Get", "WriteableControllerTests", "A")]
        [TestJson_("Get", "WriteableControllerTests", "B")]
        public void Get(string t, JsonTestCase jsonTestCase) {
            var ea = Get_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualOrWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Get", "WriteableControllerTests", "A")]
        [TestJson_("Get", "WriteableControllerTests", "B")]
        public async Task GetAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await GetAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualOrWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "WriteableControllerTests", "A")]
        [TestJson_("Delete", "WriteableControllerTests", "B")]
        public void Delete(string t, JsonTestCase jsonTestCase) {
            var ea = Delete_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualOrWrite(ea.Expected, Output));
        }


        [Theory]
        [TestJson_("Delete", "WriteableControllerTests", "A")]
        [TestJson_("Delete", "WriteableControllerTests", "B")]
        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
            Assert.True(ea.Actual.IsEqualOrWrite(ea.Expected, Output));
        }

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
