using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class UnitTestBase<TProgram, TLauncher> : IClassFixture<LauncherFixture<TProgram, TLauncher>> 
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {

        public ITestOutputHelper Output { get; }
        public HttpClient HttpClient { get; }

        public UnitTestBase(LauncherFixture<TProgram,TLauncher> fixture, ITestOutputHelper output) {
            HttpClient = fixture.HttpClient;
            Output = output;
        }

        public static IEnumerable<object[]> Data =>
                Enumerable.Range(1, 10).Select(i => new object[] { i }).ToArray();
    }
}
