using EDennis.AspNetCore.Base.Web;
using A = MockClientApi.Lib;
using I = IdentityServer.Lib;

namespace MockClientApi.Launcher {

    public class Program : ILauncher {

        public void Launch(string[] args, bool _) {
            var i = new I.Program().Run(args);
            ProgramBase.CanPingAsync(i);
            var a = new A.Program().Run(args);
            ProgramBase.CanPingAsync(a);
        }

    }
}