using System;
using I = IdentityServer.Lib;
using A = Hr.Api.Lib;
using R = Hr.RazorApp;
using EDennis.AspNetCore.Base.Web;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace Hr.Launcher {
    public class Launcher : ILauncher {

        public static void Main(string[] args) {
            new Launcher().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            var is4 = new I.Program().Run(args);
            var api = new A.Program().Run(args);
            if (args[0].Contains("=razor", StringComparison.OrdinalIgnoreCase))
                Task.Run(() => R.Program.Main(args));
            ProgramBase.CanPingAsync(is4, api);
        }

    }

}

