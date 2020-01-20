using EDennis.AspNetCore.Base.Web;
using System;
using A = Hr.Api.Lib;
using I = IdentityServer.Lib;
using R = Hr.RazorApp.Lib;

namespace Hr.Launcher {
    public class Launcher : ILauncher {

        public static bool openBrowser = false;
        public static void Main(string[] args) {
            openBrowser = true;
            new Launcher().Launch(args, true);
            LauncherUtils.Block();
        }

        public void Launch(string[] args, bool openBrowser = false) {

            var is4 = new I.Program().Run(args); //asynchronously launch IdentityServer.Lib 
            var api = new A.Program().Run(args); //asynchronously launch Hr.Api.Lib

            IProgram app = null;

            //conditionally launch the Razor app (based upon commandline arg)
            //(see launchSettings.json for how to pass commandline arguments)
            if (args.ToCommandLineArgs()["entryPoint"].Equals("Razor", StringComparison.Ordinal)) {
                app = new R.Program().Run(args);
                ProgramBase.CanPingAsync(is4, api, app);
                ProgramBase.OpenBrowser("https://localhost:44307");
            } else {
                ProgramBase.CanPingAsync(is4, api);
                ProgramBase.OpenBrowser("https://localhost:44319/swagger");
            }
        }



    }

}

