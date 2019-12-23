using EDennis.AspNetCore.Base.Web;
using M = MockClientApi.Lib;
using I = IdentityServer.Lib;

namespace MockClientApiLauncher {

    /// <summary>
    /// This launcher runs all APIs from the .Lib project's Program.Run method
    /// </summary>
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            var m = new M.Program().Run(args);
            var i = new I.Program().Run(args);
            ProgramBase.CanPingAsync(m,i);
        }


    }
}