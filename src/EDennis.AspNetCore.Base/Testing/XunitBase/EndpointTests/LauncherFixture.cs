using EDennis.AspNetCore.Base.Web;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// Xunit class fixture used to launch and terminate a web server for integration testing
    /// </summary>
    public class LauncherFixture<TProgram, TLauncher> : IDisposable
        where TProgram : IProgram, new()
        where TLauncher : ILauncher, new() {

        //the threading mechanism used to remotely terminate launcher apps
        private EventWaitHandle _ewh;

        /// <summary>
        /// An HttpClient that will be used for all tests of the entry-point application
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public IProgram Program { get; private set; }

        /// <summary>
        /// Constructs a new fixture and sets up the EventWaitHandle for 
        /// remote termination of the entry-point app
        /// </summary>
        public LauncherFixture() {
            Initialize(new string[] { });
        }


        /// <summary>
        /// Constructs a new fixture and sets up the EventWaitHandle for 
        /// remote termination of the entry-point app
        /// </summary>
        public LauncherFixture(string[] args) {
            Initialize(args);
        }

        private void Initialize(string[] args) {
            //setup the EventWaitHandle
            var arg = $"ewh={Guid.NewGuid().ToString()}";
            _ewh = new EventWaitHandle(false, EventResetMode.ManualReset, arg);

            var launcher = new TLauncher();

            //asynchronously initiate the launch of the server 
            launcher.Launch(args.Union(new string[] { arg }).ToArray());

            Program = new TProgram();

            //create the HttpClient
            HttpClient = new HttpClient {
                BaseAddress = new Uri(Program.Api.MainAddress)
            };

        }


        /// <summary>
        /// In disposing of this fixture instance, signal the EventWaitHandle so that the
        /// launcher app can terminate and then dispose of the EventWaitHandle.
        /// </summary>
        public void Dispose() {
            _ewh.Set();
            _ewh.Dispose();
            HttpClient.Dispose();
        }
    }
}
