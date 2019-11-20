using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.ExternalApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Tests {
    public class ODataQueryBuilderTests {

        private readonly ITestOutputHelper _output;

        public ODataQueryBuilderTests(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void TestBuilder1() {
            var query = new ODataQueryBuilder()
                .Select<Color>()
                .Filter("name eq blue")
                .OrderBy("name desc")
                .Top(1)
                .Skip(2)
                .Query;
            _output.WriteLine(query);
        }



    }
}
