using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class LauncherEndpointTests<TProgram,TLauncher> 
        : IClassFixture<LauncherFixture<TProgram, TLauncher>>,
        IDisposable

        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new()
        {

        protected ITestOutputHelper Output { get; }
        protected LauncherFixture<TProgram, TLauncher> LauncherFixture { get; }
        public virtual IProgram Program { get; set; }
        protected Apis Apis { get; }
        public virtual string InstanceName { get; } = Guid.NewGuid().ToString();
        public const string DEFAULT_USER = "tester@example.org";

        public HttpClient HttpClient { get; }

        public LauncherEndpointTests(ITestOutputHelper output,
            LauncherFixture<TProgram, TLauncher> launcherFixture) {
            Output = output;
            LauncherFixture = launcherFixture;
            HttpClient = launcherFixture.HttpClient;

            HttpClient.DefaultRequestHeaders.Add(Constants.ENTRYPOINT_KEY, EntryPoint.TestProject.ToString());
            HttpClient.DefaultRequestHeaders.Add(Constants.USER_KEY, DEFAULT_USER);

            Program = launcherFixture.Program;
            Apis = launcherFixture.Program.Apis;
        }


        public void Dispose() {
            //MiddlewareUtils.Reset(Apis,InstanceName);
        }

    }
}
