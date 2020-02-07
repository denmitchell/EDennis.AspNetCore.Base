using EDennis.AspNetCore.Base.Web;
using System;
using A = Hr.Api.Lib;
using I = IdentityServer.Lib;
using R = Hr.RazorApp.Lib;

namespace Hr.Launcher {
    public class Launcher : ILauncher {

        /// <summary>
        /// Entry point when developer launches via 
        /// green arrow Run button
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {
            new Launcher().Launch(args, true);
            LauncherUtils.Block();
        }

        /// <summary>
        /// Entry point for automated unit tests
        /// </summary>
        /// <param name="args"></param>
        /// <param name="openBrowser"></param>
        public void Launch(string[] args, bool openBrowser = false) {

            var is4 = new I.Program().Run(args); //async. launch IdentityServer.Lib 
            var api = new A.Program().Run(args); //async. launch Hr.Api.Lib

            //conditionally launch the User app (based upon commandline arg)
            //(see launchSettings.json for how to pass commandline arguments)
            if (args.ToCommandLineArgs()["entryPoint"]
                    .Equals("UserApp", StringComparison.Ordinal)) {
                var app = new R.Program().Run(args);
                ProgramBase.CanPingAsync(is4, api, app);
                ProgramBase.OpenBrowser("https://localhost:44307");
            } else {
                ProgramBase.CanPingAsync(is4, api);
                ProgramBase.OpenBrowser("https://localhost:44319/swagger");
            }
        }

    }

}

