//using Colors2.Models;
//using Colors2Api.Lib.Controllers;
//using EDennis.AspNetCore.Base.Testing;
//using EDennis.NetCoreTestingUtilities;
//using EDennis.NetCoreTestingUtilities.Extensions;
//using System.Threading.Tasks;
//using Xunit;
//using Xunit.Abstractions;

//namespace Colors2Api.ControllerTests {

//    [Collection("Controller Tests")]
//    public class WriteableControllerTests : SqlServerWriteableControllerTests<RgbController,RgbRepo,Rgb,Color2DbContext>
//        {
        
//        public WriteableControllerTests(ITestOutputHelper output) : base(output) {
//        }

//        public readonly string[] propsToIgnore = new string[] { "SysStart", "SysEnd" };


//        internal class TestJson_ : TestJsonAttribute {
//            public TestJson_(string methodName, string testScenario, string testCase)
//                : base("Color2Db", "Colors2Api", "RgbController", methodName, testScenario, testCase) {
//            }
//        }


//        //NOTE: OData is endpoint testable only


//        [Theory]
//        [TestJson_("GetDevExtreme", "WriteableControllerTests|FilterSkipTake", "A")]
//        [TestJson_("GetDevExtreme", "WriteableControllerTests|FilterSortSelectTake", "B")]
//        public void GetDevExtreme(string t, JsonTestCase jsonTestCase) {
//            var ea = GetDevExtreme_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
//        }


//        [Theory]
//        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereSkipTake", "A")] //RedGt200Skip2Take5
//        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereOrderBySelectTake", "B")] //NameContainsBlueSelectNameDescSysUserTake10
//        public void GetDynamicLinq(string t, JsonTestCase jsonTestCase) {
//            var ea = GetDynamicLinq_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
//        }

//        [Theory]
//        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereSkipTake", "A")] //RedGt200Skip2Take5
//        [TestJson_("GetDynamicLinq", "WriteableControllerTests|WhereOrderBySelectTake", "B")] //NameContainsBlueSelectNameDescSysUserTake10
//        public async Task GetDynamicLinqAsync(string t, JsonTestCase jsonTestCase) {
//            var ea = await GetDynamicLinqAsync_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, Output));
//        }


//        [Theory]
//        [TestJson_("Get", "WriteableControllerTests", "A")]
//        [TestJson_("Get", "WriteableControllerTests", "B")]
//        public void Get(string t, JsonTestCase jsonTestCase) {
//            var ea = Get_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Get", "WriteableControllerTests", "A")]
//        [TestJson_("Get", "WriteableControllerTests", "B")]
//        public async Task GetAsync(string t, JsonTestCase jsonTestCase) {
//            var ea = await GetAsync_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Delete", "WriteableControllerTests", "A")]
//        [TestJson_("Delete", "WriteableControllerTests", "B")]
//        public void Delete(string t, JsonTestCase jsonTestCase) {
//            var ea = Delete_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Delete", "WriteableControllerTests", "A")]
//        [TestJson_("Delete", "WriteableControllerTests", "B")]
//        public async Task DeleteAsync(string t, JsonTestCase jsonTestCase) {
//            var ea = await DeleteAsync_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Post", "WriteableControllerTests", "A")]
//        [TestJson_("Post", "WriteableControllerTests", "B")]
//        public void Post(string t, JsonTestCase jsonTestCase) {
//            Output.WriteLine(t);
//            var ea = Post_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Post", "WriteableControllerTests", "A")]
//        [TestJson_("Post", "WriteableControllerTests", "B")]
//        public async Task PostAsync(string t, JsonTestCase jsonTestCase) {
//            var ea = await PostAsync_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Put", "WriteableControllerTests", "A")]
//        [TestJson_("Put", "WriteableControllerTests", "B")]
//        public void Put(string t, JsonTestCase jsonTestCase) {
//            Output.WriteLine(t);
//            var ea = Put_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//        [Theory]
//        [TestJson_("Put", "WriteableControllerTests", "A")]
//        [TestJson_("Put", "WriteableControllerTests", "B")]
//        public async Task PutAsync(string t, JsonTestCase jsonTestCase) {
//            var ea = await PutAsync_ExpectedActual(t, jsonTestCase);
//            Assert.True(ea.Actual.IsEqualAndWrite(ea.Expected, 3, propsToIgnore, Output, true));
//        }


//    }
//}
