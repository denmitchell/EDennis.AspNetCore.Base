using EDennis.AspNetCore.Base.Web;
using System;
using I = IdentityServer.Lib;
using C = ConfigurationApi.Lib;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace IdentityServer.Launcher {

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
            using var client = new HttpClient();
            var response = client.GetAsync("https://localhost:44312/api/Alive").Result;

            while (!response.IsSuccessStatusCode) {
                Thread.Sleep(1000);
                response = client.GetAsync("https://localhost:44312/api/Alive").Result;
            }
            var iApi = new I.Program().Run(args);

        }

    }

}

