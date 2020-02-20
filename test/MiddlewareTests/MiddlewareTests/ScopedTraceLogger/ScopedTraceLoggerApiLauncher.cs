using EDennis.AspNetCore.Base.Web;
using L = EDennis.Samples.ScopedLoggerMiddlewareApi.Lib;

namespace EDennis.Samples.ScopedLoggerMiddlewareApi.Launcher {

    /// <summary>
    /// This launcher runs all APIs needed for the test
    /// </summary>
    public class Program : ILauncher {

        public void Launch(string[] args, bool _) {
            var l = new L.Program().Run(args);
            ProgramBase.CanPingAsync(l);
        }

    }
}