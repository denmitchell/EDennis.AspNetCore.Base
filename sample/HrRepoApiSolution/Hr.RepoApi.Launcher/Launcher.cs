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

            var cApi = new C.Program().Run(args);
            ProgramBase.CanPingAsync(cApi);

            var iApi = new I.Program().Run(args);
            ProgramBase.CanPingAsync(iApi);

            var aApi = new A.Program().Run(args);
            ProgramBase.CanPingAsync(aApi);

            if (openBrowser) 
                ProgramBase.OpenBrowser("https://localhost:44319/swagger");

        }

    }

}

