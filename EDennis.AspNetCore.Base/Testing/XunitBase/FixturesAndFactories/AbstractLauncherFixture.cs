using EDennis.AspNetCore.Base.Web;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// Subclass this abstract fixture if using Xunit with the ApiLauncher pattern
    /// Xunit class fixture used to launch and terminate a web server for integration testing
    /// </summary>
    public abstract class AbstractLauncherFixture : ConfigurationFixture, IDisposable {

        //the threading mechanism used to remotely terminate launcher apps
        private readonly EventWaitHandle _ewh;

        public virtual string InstanceName { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The entry-point application's scheme (can be overidden in subclass)
        /// </summary>
        public virtual string EntryPointScheme { get; } = "https";

        /// <summary>
        /// The entry-point application's port (must be overidden in subclass)
        /// </summary>
        public abstract int EntryPointPort { get; }

        /// <summary>
        /// The Launcher's Main method (must be overidden in subclass)
        /// </summary>
        public abstract Action<string[]> LauncherMain { get; }

        /// <summary>
        /// An HttpClient that will be used for all tests of the entry-point application
        /// </summary>
        public HttpClient HttpClient { get; set; } 

        /// <summary>
        /// Constructs a new fixture and sets up the EventWaitHandle for 
        /// remote termination of the entry-point app
        /// </summary>
        public AbstractLauncherFixture() {

            //setup the EventWaitHandle
            var arg = $"ewh={Guid.NewGuid().ToString()}";
            _ewh = new EventWaitHandle(false, EventResetMode.ManualReset, arg);

            //create the HttpClient
            HttpClient = new HttpClient {
                BaseAddress = new Uri($"{EntryPointScheme}://localhost:{EntryPointPort}")
            };
            HttpClient.DefaultRequestHeaders.Add(Constants.TESTING_INSTANCE_KEY, InstanceName);

            //asynchronously initiate the launch of the server 
            Task.Run(() => { LauncherMain(new string[] { arg }); });

            //optional : use custom PingAsync (see HttpClientExtensions) to wait for server to start.
            var canPing = HttpClient.PingAsync(10).Result;

        }

        /// <summary>
        /// In disposing of this fixture instance, signal the EventWaitHandle so that the
        /// launcher app can terminate and then dispose of the EventWaitHandle.
        /// </summary>
        public virtual new void Dispose() {
            base.Dispose();
            _ewh.Set();
            _ewh.Dispose();
        }
    }
}
