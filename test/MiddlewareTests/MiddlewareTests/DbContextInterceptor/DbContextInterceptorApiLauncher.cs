using EDennis.AspNetCore.Base.Web;
using L = EDennis.Samples.DbContextInterceptorMiddlewareApi.Lib;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi.Launcher {

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