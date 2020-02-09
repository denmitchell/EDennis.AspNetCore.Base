using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Hr.RepoApi {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new Lib.Program().CreateHostBuilder(args);
    }
}
