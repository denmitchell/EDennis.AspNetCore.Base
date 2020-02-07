using ConfigurationApi.Lib.Models;
using EDennis.AspNetCore.Base.Web;
using System;
using System.Threading.Tasks;
using Internal = ConfigurationApi.Lib;

namespace Colors2Launcher {
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

            var iApi = new Internal.Program().Run(args);

            if (args.ToCommandLineArgs().TryGetValue("uploadConfigs",out string uploadConfigs)) {
                if (uploadConfigs.Equals("true", StringComparison.OrdinalIgnoreCase)) {
                    var configurationManager = new ConfigurationManager();
                    Task.Run(async () => { await configurationManager.UploadNew(); });
                }

            }

            if (openBrowser) {
                ProgramBase.CanPingAsync(iApi);
                ProgramBase.OpenBrowser("https://localhost:44312/swagger");
            }
        }

    }

}

