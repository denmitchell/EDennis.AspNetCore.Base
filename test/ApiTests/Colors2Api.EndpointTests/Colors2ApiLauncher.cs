using EDennis.AspNetCore.Base.Web;
using M = Colors2Api.Lib;

namespace Colors2Api.EndpointTests {
    public class Colors2ApiLauncher : ILauncher {
        public void Launch(string[] args) {
            var m = new M.Program().Run(args);
            ProgramBase.CanPingAsync(m);
        }
    }
}