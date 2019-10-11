using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using EDennis.AspNetCore.Base.Security;
using System.Collections.Concurrent;
using Moq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

namespace EDennis.AspNetCore.Base.Security {
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
            var mockLogger = new Mock<ILogger>();
            var handler = new ClaimPatternAuthorizationHandler(policyScope,
                new ScopePatternOptions()/*use defaults*/, new ConcurrentDictionary<string, MatchType>(),
                mockLogger.Object);
            var actual = handler.EvaluatePattern(policyScope, scopeClaims);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(true, true, "A.B", new string[] { "A.B" }, MatchType.Positive)]
        [InlineData(true, false, "A.B", new string[] { "-A.B" }, MatchType.Negative)]
        [InlineData(false, true, "A.B", new string[] { "-A.C" }, MatchType.Positive)]
        [InlineData(false, false, "A.B", new string[] { "-A*" }, MatchType.Negative)]
        [InlineData(true, true, "A.B", new string[] { "-A*,A.B" }, MatchType.Positive)]
        [InlineData(true, false, "A.B", new string[] { "A.*,-A.C" }, MatchType.Positive)]
        [InlineData(false, true, "A.B", new string[] { "A.*,-A.B" }, MatchType.Negative)]
        [InlineData(false, false, "A.B.C", new string[] { "A.*,-A.B*,A.B.C" }, MatchType.Positive)]
        [InlineData(true, true, "A.B.C", new string[] { "A.*,-A.B*,A.B.D" }, MatchType.Negative)]
        [InlineData(true, false, "A.B.C", new string[] { "-A.*,A.B*,-A.B.C" }, MatchType.Negative)]
        [InlineData(false, true, "A.B.C", new string[] { "-A.*,A.B*,-A.B.D" }, MatchType.Positive)]
        [InlineData(false, false, "A.B", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Negative)]
        [InlineData(true, true, "A.C", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Positive)]
        [InlineData(true, false, "B.C", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Negative)]
        [InlineData(false, true, "B.D", new string[] { "A.*,-A.B", "B.*,-B.C" }, MatchType.Positive)]
        public void TestEvaluateScope(bool isOidc, bool inCache, string policyScope, string[] scopeClaims, MatchType expected) {

            var scopePatternOptions = new ScopePatternOptions { IsOidc = isOidc };


            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.SetupGet(cp => cp.Claims).Returns(
                scopeClaims.Select(c => new Claim($"{(isOidc ? scopePatternOptions.UserScopePrefix : "")}scope", c)));

            var cache = new ConcurrentDictionary<string, MatchType>();
            if (inCache) {
                scopeClaims.ToList().ForEach(sc => {
                    cache.GetOrAdd(sc, expected);
                });
            }

            var mockLogger = new Mock<ILogger>();

            var handler = new ClaimPatternAuthorizationHandler(policyScope, scopePatternOptions, cache, mockLogger.Object);

            var actual = handler.EvaluateScope(mockClaimsPrincipal.Object, handler);
            Assert.Equal(expected, actual);

            mockLogger.Verify(c => c.Log(LogLevel.Trace,
                It.IsAny<EventId>(),
                new FormattedLogValues("Scope claim matches policy pattern", null),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
                (expected == MatchType.Positive) ? Times.Once() : Times.Never());


            mockLogger.Verify(c => c.Log(LogLevel.Trace,
                It.IsAny<EventId>(),
                new FormattedLogValues("Scope claim matches cached policy pattern", null),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
                inCache ? Times.Once() : Times.Never());


        }

    }
}
