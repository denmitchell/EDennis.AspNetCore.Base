using EDennis.AspNetCore.Base.Web;
using L = EDennis.Samples.HeadersToClaimsMiddlewareApi.Lib;

namespace EDennis.Samples.HeadersToClaimsMiddlewareApi.Launcher {

    /// <summary>
    /// This launcher runs all necessary APIs for a test
    /// </summary>
    public class Program : ILauncher {

        public void Launch(string[] args, bool _) {
            var l = new L.Program().Run(args);
            ProgramBase.CanPingAsync(l);
        }

    }
}