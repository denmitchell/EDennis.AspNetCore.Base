using EDennis.AspNetCore.Base.Web;
using A = ScopedLoggerApi.Lib;

namespace ScopedLoggerApi.Launcher {
    public class Program : ILauncher {
        public void Launch(string[] args, bool _) {
            var a = new A.Program().Run(args);
            ProgramBase.CanPingAsync(a);
        }

    }
}