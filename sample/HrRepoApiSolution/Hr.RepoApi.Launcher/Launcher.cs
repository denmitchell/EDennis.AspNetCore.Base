using EDennis.AspNetCore.Base.Web;
using System;
using I = IdentityServer.Lib;
using C = ConfigurationApi.Lib;
using A = Hr.RepoApi.Lib;

namespace Hr.RepoApi.Launcher {

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

            var iApi = new I.Program().Run(args);
            var cApi = new C.Program().Run(args);
            var aApi = new A.Program().Run(args);

            //conditionally launch the External Swagger UI (based upon commandline arg)
            //(see launchSettings.json for how to pass commandline arguments)
            if (args.ToCommandLineArgs()["entryPoint"]
                    .Equals("External", StringComparison.Ordinal)) {
                var eApi = new External.Program().Run(args);
                ProgramBase.CanPingAsync(iApi, eApi);
                ProgramBase.OpenBrowser("https://localhost:44358/swagger");
            } else {
                ProgramBase.CanPingAsync(iApi);
                ProgramBase.OpenBrowser("https://localhost:44352/swagger");
            }
        }

    }

}

