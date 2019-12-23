using EDennis.AspNetCore.Base.Web;
using M = EDennis.Samples.MockClientMiddlewareApi.Lib;
using I = IdentityServer.Lib;
using System.Threading.Tasks;

namespace EDennis.Samples.MockClientMiddlewareApi.Launcher {

    /// <summary>
    /// This launcher runs all APIs from the .Lib project's Program.Run method
    /// </summary>
    public class Program : ILauncher {

        public void Launch(string[] args) {
            var m = new M.Program().Run(args);
            var i = new I.Program().Run(args);
            ProgramBase.CanPingAsync(m, i);
        }


        //public void Launch(string[] args) {
        //    Task.Run(() => M.Program.Main(args));
        //    Task.Run(() => I.Program.Main(args));
        //}

    }
}