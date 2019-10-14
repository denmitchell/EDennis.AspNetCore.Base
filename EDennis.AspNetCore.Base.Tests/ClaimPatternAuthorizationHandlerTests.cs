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
using Microsoft.Extensions.Logging.Abstractions;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.AspNetCore.Base.Security {
    public class ClaimPatternAuthorizationHandlerTests {

        ITestOutputHelper _output;

        public ClaimPatternAuthorizationHandlerTests(ITestOutputHelper output) {
            _output = output;
        }

        [Theory]
        [InlineData("AB", "AB", true)]
        [InlineData("AB", "AC", false)]
        [InlineData("AB", "A*", true)]
        [InlineData("BC", "A*", false)]
        [InlineData("BC", "*", true)]
        [InlineData("ABC", "A*C", true)]
        [InlineData("ABC", "A*B", false)]
        [InlineData("ABCDEFGHI", "A*EFG*I", true)]
        [InlineData("ABCDBFGHI", "A*EFG*I", false)]
        [InlineData("ABCDEFGHI", "A*D*EFG*I", true)]
        [InlineData("ABCDEFGHI", "A*D*EFG*I*", true)]
        public void TestMatchesWilcardPattern(string input, string pattern, bool expected) {
            var actual = input.MatchesWildcardPattern(pattern);
            Assert.Equal(expected, actual);
        }

        internal enum IO {
            I, O
        }
        static readonly MatchType we = MatchType.WildcardExclusion;
        static readonly MatchType wi = MatchType.WildcardInclusion;
        static readonly MatchType ne = MatchType.NonWildcardExclusion;
        static readonly MatchType ni = MatchType.NonWildcardInclusion;

        static Dictionary<IO, MatchType[]> D(params MatchType[] matchTypes) =>
            new Dictionary<IO, MatchType[]> {
                { IO.I, matchTypes.Take(matchTypes.Length / 2).ToArray() }, //first half is input
                { IO.O, matchTypes.Skip(matchTypes.Length / 2).ToArray() }, //second half is output
            };

        static readonly Dictionary<IO, MatchType[]>[] TestEvaluatePatternData = new Dictionary<IO, MatchType[]>[] {
            D(we,we,ni,ni),/*0*/    
            D(wi,wi,ne,ne),/*1*/    
            D(wi,wi,wi,wi),/*2*/    
            D(wi,wi,ne,wi),/*3*/        D(ne,wi,ni,ni),/*4*/
            D(we,we,ni,wi),/*5*/        D(ni,wi,ni,wi),/*6*/
                                        D(ni,wi,ni,ne),/*7*/
            D(we,we,we,ni,wi,wi),/*8*/  D(ni,wi,wi,ni,ne,wi),/*9*/     D(ne,ne,wi,ni,ni,ni),/*10*/
                                                                        D(ne,ne,wi,ne,ne,wi),/*11*/
            D(wi,wi,wi,ne,wi,wi),/*12*/ D(ne,wi,wi,ni,ni,wi),/*13*/     D(ni,ni,wi,ni,ni,ne),/*14*/
                                                                        D(ni,ni,wi,ni,ni,wi),/*15*/
         };

        [Theory]
        [InlineData("AB", "AB", 0)]
        [InlineData("AB", "-AB", 1)]
        [InlineData("AB", "-AC", 2)]
        [InlineData("AB", "-A*", 3)]
        [InlineData("AB", "AB", 4)]
        [InlineData("AB", "A*", 5)]
        [InlineData("AB", "-AC", 6)]
        /*[InlineData("AB", "A*", 5)]*/
        [InlineData("AB", "-AB", 7)]
        [InlineData("ABC", "A*", 8)]
        [InlineData("ABC", "-AB*", 9)]
        [InlineData("ABC", "ABC", 10)]
        /*[InlineData("ABC", "A*", 8)]   [InlineData("ABC", "-AB*", 9)]*/
        [InlineData("ABC", "ABD", 11)]
        [InlineData("ABC", "-A*", 12)]
        [InlineData("ABC", "AB*", 13)]
        [InlineData("ABC", "-ABC", 14)]
        /*[InlineData("ABC", "-A*", 12)] [InlineData("ABC", "AB*", 13)]*/
        [InlineData("ABC", "-ABD", 15)]
        public void TestEvaluatePattern(string policyScope, string scopeClaim, int testEvaluatePatternIndex) {

            var data = TestEvaluatePatternData[testEvaluatePatternIndex];

            var mockLogger = new Mock<ILogger>();
            var handler = new ClaimPatternAuthorizationHandler(policyScope,
                new ScopePatternOptions()/*use defaults*/, new ConcurrentDictionary<string, bool>(),
                mockLogger.Object);

            var input = data[IO.I];
            var expected = data[IO.O];
            handler.EvaluatePattern(policyScope, scopeClaim, ref input);

            Assert.Equal(expected, input); //input = actual after processing
        }


        [Theory]
        [InlineData("AB", "AB", true)]
        [InlineData("AB", "-AB", false)]
        [InlineData("AB", "-AC", true)]
        [InlineData("AB", "-A*,AB", true)]
        [InlineData("AB", "A*,-AC", true)]
        [InlineData("AB", "A*,-AB", false)]
        [InlineData("ABC", "A*,-AB*,ABC", true)]
        [InlineData("ABC", "A*,-AB*,ABD", false)]
        [InlineData("ABC", "-A*,AB*,-ABC", false)]
        [InlineData("ABC", "-A*,AB*,-ABD", true)]
        public void TestEvaluateScopeClaim(string requirement, string scopeClaim, bool expected) {
            var mockLogger = new Mock<ILogger>();
            var handler = new ClaimPatternAuthorizationHandler(requirement,
                new ScopePatternOptions()/*use defaults*/, new ConcurrentDictionary<string, bool>(),
                mockLogger.Object);

            var actual = handler.EvaluateScopeClaim(requirement, scopeClaim);
            Assert.Equal(expected, actual);
        }



        static readonly string[] logMessages = new string[] {
            ".*is cached.*", ".*evaluating.*"
        };

        [Theory]
        [InlineData("AB", new string[] { "AB" }, true, true, null, new int[] { 0, 1 })]
        [InlineData("AB", new string[] { "-AB" }, false, false, new bool[] { false }, new int[] { 1, 0 })]
        [InlineData("AB", new string[] { "-AC" }, true, true, null, new int[] { 0, 1 })]
        [InlineData("AB", new string[] { "-A*" }, true, false, new bool[] { true }, new int[] { 1, 0 })]
        [InlineData("AB", new string[] { "-A*,AB" }, true, true, null, new int[] { 0, 1 })]
        [InlineData("AB", new string[] { "A*,-AC" }, true, false, new bool[] { true }, new int[] { 1, 0 })]
        [InlineData("AB", new string[] { "A*,-AB" }, false, true, null, new int[] { 0, 1 })]
        [InlineData("ABC", new string[] { "A*,-AB*,ABC" }, true, false, new bool[] { true }, new int[] { 1, 0 })]
        [InlineData("ABC", new string[] { "-A*,AB*,-ABC" }, false, true, null, new int[] { 0, 1 })]
        [InlineData("AB", new string[] { "A*,-AB", "B*,-BC" }, false, false, new bool[] { false, false }, new int[] { 2, 0 })]
        [InlineData("BC", new string[] { "A*,-AB", "B*,-BC" }, false, true, null, new int[] { 0, 2 })]
        [InlineData("AC", new string[] { "A*,-AB", "B*,-BC" }, true, false, new bool[] { true, false }, new int[] { 1, 0 })] //passes immediately, so cache called just once
        [InlineData("BD", new string[] { "A*,-AB", "B*,-BC" }, true, true, null, new int[] { 0, 2 })]
        public void TestEvaluateScope(string policyScope, string[] scopeClaims,
            bool expected, bool isOidc, bool[] cachedValues, int[] expectedLogMessageFrequency) {

            var scopePatternOptions = new ScopePatternOptions { IsOidc = isOidc };
            var claimType = $"{(isOidc ? scopePatternOptions.UserScopePrefix : "")}scope";

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            mockClaimsPrincipal.SetupGet(cp => cp.Claims).Returns(
                scopeClaims.Select(c => new Claim(claimType, c)));

            var cache = new ConcurrentDictionary<string, bool>();
            if (cachedValues != null)
                for (int i = 0; i < cachedValues.Length; i++)
                    cache.TryAdd(scopeClaims[i], cachedValues[i]);

            var mockLogger = new Mock<TestLogger>();
            mockLogger.SetupGet(m => m.InternalLogger).Returns(new Mock<ILogger>().Object);
            mockLogger.SetupGet(m => m.TestOutputHelper).Returns(_output);

            var handler = new ClaimPatternAuthorizationHandler(policyScope, scopePatternOptions, cache, mockLogger.Object);


            var actual = handler.EvaluateClaimsPrincipal(mockClaimsPrincipal.Object, handler);
            Assert.Equal(expected, actual);


            for (int i = 0; i < logMessages.Length; i++)
                mockLogger.Verify(c => c.XLog(LogLevel.Trace,
                    It.IsRegex(logMessages[i])), Times.Exactly(expectedLogMessageFrequency[i]));

        }

    }
}