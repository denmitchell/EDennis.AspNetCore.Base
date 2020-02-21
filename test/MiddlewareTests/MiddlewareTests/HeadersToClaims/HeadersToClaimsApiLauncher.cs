using EDennis.AspNetCore.Base.Web;
using A = HeadersToClaimsApi.Lib;

namespace HeadersToClaimsApi.Launcher {

    public class Program : ILauncher {

        public void Launch(string[] args, bool _) {
            var a = new A.Program().Run(args);
            ProgramBase.CanPingAsync(a);
        }

    }
}