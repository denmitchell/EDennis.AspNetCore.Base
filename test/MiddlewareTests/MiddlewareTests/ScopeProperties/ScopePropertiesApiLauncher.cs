using EDennis.AspNetCore.Base.Web;
using L = EDennis.Samples.ScopePropertiesMiddlewareApi.Lib;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopePropertiesMiddlewareApi.Launcher {

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