using Microsoft.AspNetCore.Hosting;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This class provides an extension method to IWebHostBuilder
    /// that creates a new ApiLauncher, launches an configured Apis,
    /// waits for the main application to terminate, and then
    /// terminates the configured Apis.  NOTE: the launcher
    /// is configured to run in Development onlyl
    /// </summary>
    public static class IWebHostBuilderExtensions {

        /// <summary>
        /// When in Development, creates a new ApiLauncher,
        /// starts configured Apis, runs the main application,
        /// and stops all Apis after the main application
        /// terminates; otherwise, it just runs the main 
        /// applications
        /// </summary>
        /// <param name="builder"></param>
        public static void BuildAndRunWithLauncher(this IWebHostBuilder builder) {

            ApiLauncher launcher = null;

            //if in Development, launch Apis
            if (builder.GetSetting("Environment") == EnvironmentName.Development) {
                launcher = new ApiLauncher();
                launcher.StartApis(builder);
            }

            //run the main application
            builder.Build().Run();

            //upon termination of the main application,
            //stop all launched Apis.
            if (launcher != null)
                launcher.StopApis();
        }

    }
}
