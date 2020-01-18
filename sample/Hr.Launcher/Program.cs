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

            var is4 = new I.Program().Run(args); //asynchronously launch IdentityServer.Lib 
            var api = new A.Program().Run(args); //asynchronously launch Hr.Api.Lib

            //conditionally launch the Razor app (based upon commandline arg)
            //(see launchSettings.json for how to pass commandline arguments)
            if (args.ToCommandLineArgs()["entryPoint"].Equals("Razor",StringComparison.Ordinal) )
                Task.Run(() => R.Program.Main(args)); //asynchronously launch Hr.RazorApp

            ProgramBase.CanPingAsync(is4, api);
        }

    }

}

