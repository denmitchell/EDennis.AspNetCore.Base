using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Base.Security;
using System.Collections.Concurrent;

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

        [Theory]
        [InlineData("A.B", new string[] { "A.B" }, MatchType.Positive)]
        [InlineData("A.B", new string[] { "-A.B" }, MatchType.Negative)]
        [InlineData("A.B", new string[] { "-A.C" }, MatchType.Positive)]
        [InlineData("A.B", new string[] { "-A*" }, MatchType.Negative)]
        [InlineData("A.B", new string[] { "-A*,A.B" }, MatchType.Positive)]
        [InlineData("A.B", new string[] { "A.*,-A.C" }, MatchType.Positive)]
        [InlineData("A.B", new string[] { "A.*,-A.B" }, MatchType.Negative)]
        [InlineData("A.B.C", new string[] { "A.*,-A.B*,A.B.C" }, MatchType.Positive)]
        [InlineData("A.B.C", new string[] { "A.*,-A.B*,A.B.D" }, MatchType.Negative)]
        [InlineData("A.B.C", new string[] { "-A.*,A.B*,-A.B.C" }, MatchType.Negative)]
        [InlineData("A.B.C", new string[] { "-A.*,A.B*,-A.B.D" }, MatchType.Positive)]
        [InlineData("A.B", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Negative)]
        [InlineData("A.C", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Positive)]
        [InlineData("B.C", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Negative)]
        [InlineData("B.D", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Positive)]
        public void TestEvaluatePattern(string policyScope, string[] scopeClaims, MatchType expected) {
            var handler = new ClaimPatternAuthorizationHandler(policyScope,
                new ScopePatternOptions()/*use defaults*/, new ConcurrentDictionary<string, MatchType>());
            var actual = handler.EvaluatePattern(policyScope,scopeClaims);
            Assert.Equal(expected, actual);
        }


    }
}
