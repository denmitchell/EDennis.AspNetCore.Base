using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Base.Security;

namespace EDennis.AspNetCore.Base.Tests {
    public class ClaimPatternAuthorizationHandlerTests {

        [Theory]
        [InlineData("A.B", "A.B", true)]
        [InlineData("A.B", "A.C", false)]
        [InlineData("A.B", "A.*", true)]
        [InlineData("B.C", "A.*", false)]
        [InlineData("B.C", "*", true)]
        [InlineData("A.B.C", "A.*.C", true)]
        [InlineData("A.B.C", "A.*.B", false)]
        [InlineData("A.BCDE.FGHI", "A.*E.FG*I", true)]
        [InlineData("A.BCDB.FGHI", "A.*E.FG*I", false)]
        [InlineData("A.BCDE.FGHI", "A.*D*E.FG*I", true)]
        [InlineData("A.BCDE.FGHI", "A.*D*E.FG*I*", true)]
        public void TestMatchesWilcardPattern(string input, string pattern, bool expected) {
            var actual = input.MatchesWildcardPattern(pattern);
            Assert.Equal(expected, actual);
        }
    }
}
