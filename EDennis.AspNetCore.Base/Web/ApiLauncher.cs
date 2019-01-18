using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    /// <summary>
    /// This class is a testing tool -- used to launch 
    /// one or more Api dependencies when in Development.  
    /// 
    /// To use the ApiLauncher ...
    /// (1) Ensure that all projects are in the default
    /// directory structure: c:\users\{USER_PROFILE}\sources\repos\{SolutionName}\{ProjectName};
    /// otherwise, you must use the ProjectFullPath property in
    /// appsettings.Development.json.
    /// (2) Add an Apis section to appsettings.Development.json.  EXAMPLE:
    ///   "Apis": {
    ///       "IdentityServer": {
    ///           "SolutionName": "EDennis.AspNetCore.Base",
    ///           "ProjectName": "IdentityServer",
    ///           "Port": 5006
    ///       },
    ///       "InternalApi1": {
    ///           "LaunchProfile":  "AltProfile",
    ///           "SolutionName": "EDennis.AspNetCore.Base",
    ///           "ProjectName": "EDennis.Samples.InternalApi1",
    ///           "Port": 5007
    ///       }
    ///   }
    /// (3) Ensure that appsettings.Development.json is marked as
    ///     Copy always or Copy if newer.
    /// (4) In Program.cs, replace the Main method body with the
    ///     following:
    ///         CreateWebHostBuilder(args).BuildAndRunWithLauncher();
    /// </summary>
    public class ApiLauncher : IDisposable {

        //holds references to all launched APIs
        private Dictionary<int, Api> _runningApis
            = new Dictionary<int, Api>();

        //used to synchronize access to the Console's title bar.
        private static object _lockObj = new object();


        /// <summary>
        /// Starts the APIs using the Program class's IWebHostBuilder
        /// to obtain the Environment name and port setting
        /// </summary>
        /// <param name="builder">Program class's IWebHostBuilder</param>
        public void StartApis(IWebHostBuilder builder) {

            //get the port setting (does not work with IIS)
            var portSetting = builder.GetSetting("Urls");
            if (portSetting != null) {
                var port = portSetting.Replace("https", "").Replace("http", "").Replace("://localhost:", "");

                lock (_lockObj) {
                    Console.Title += $"({port})";
                }
            }

            //read in appsettings.Development.json
            var config = new ConfigurationBuilder();
            config.SetBasePath(AppContext.BaseDirectory);
            config.AddJsonFile("appsettings.Development.json");

            //start the APIs, passing in the configuration
            StartApis(config.Build());
        }


        /// <summary>
        /// Starts all APIs referenced in configuration
        /// </summary>
        /// <param name="config">Configuration holding data for APIs</param>
        public void StartApis(IConfiguration config) {

            //get the API data from the configuration
            var apiSections = config.GetSection("Apis").GetChildren();

            //iterate over all API data
            foreach (var apiSection in apiSections) {
                //build a dictionary of controller URLs (usually just one)
                var controllerUrls = apiSection.GetSection("ControllerUrls").GetChildren()
                    .ToDictionary(cu => cu.Key, cu => cu.Value);

                //create an API object to hold the data
                var api = new Api {
                    ApiName = apiSection.Key,
                    SolutionName = apiSection["SolutionName"],
                    ProjectName = apiSection["ProjectName"],
                    FullProjectPath = apiSection["FullProjectPath"],
                    Port = int.Parse(apiSection["Port"]),
                    LaunchProfile = apiSection["LaunchProfile"]
                };

                //start the API
                StartApi(api);
            }
        }


        /// <summary>
        /// Starts the API in a new thread
        /// </summary>
        /// <param name="api">The Api to start</param>
        private void StartApi(Api api) {

            //if LaunchProfile has been set, create dotnet run param for it.
            var launchProfileArg = "--no-launch-profile";
            if (api.LaunchProfile != null)
                launchProfileArg = $"--launch-profile {api.LaunchProfile}";

            //configure a background process for running dotnet,
            //ensuring that the port is set appropriately and
            //that all console output is to the same console
            var info = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/c SET ASPNETCORE_URLS=http://localhost:{api.Port} && dotnet run {launchProfileArg} --no-build --ASPNETCORE_URLS=http://localhost:{api.Port}",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,

                WorkingDirectory = api.LocalProjectDirectory
            };

            //call the dotnet run command asynchronously
            Task.Run(() => {
                Process p = new Process();
                p.StartInfo = info;
                p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                p.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                //get the port number, if available
                var port = (api.LaunchProfile == null) ? $"({api.Port.ToString()})" : "";

                //update the console title to add the launched API
                lock (_lockObj) {
                    Console.Title += $", {api.ApiName}{port}";
                }

                //add the launched Api to the dictionary of running APIs
                api.Process = p;
                _runningApis.Add(api.Port, api);

                //wait for the process to be suspended.
                p.WaitForExit();
            });
        }

        /// <summary>
        /// Gets the last segment in a network/folder path
        /// </summary>
        /// <param name="path">the source path</param>
        /// <returns>the last segment of the source path</returns>
        private static string GetLastSegment(string path) {
            int index = path.LastIndexOf("\\");
            if (index == -1)
                return null;
            var proj = path.Substring(index + 1);
            index = proj.LastIndexOf(".");
            if (index == -1)
                return proj;
            else
                return proj.Substring(index + 1);
        }

        /// <summary>
        /// Writes output to the console
        /// </summary>
        /// <param name="sendingProcess">the process sending the data</param>
        /// <param name="args">the data to write to the console</param>
        static void OutputHandler(object sendingProcess, DataReceivedEventArgs args) {
            Console.WriteLine(args.Data);
        }

        /// <summary>
        /// Stop a specific API
        /// </summary>
        public void StopApi(int port) {
            var api = _runningApis[port];
            //stop the server
            api.Process.Close();
            //remove the server from the dictionary of running servers
            _runningApis.Remove(port);
        }


        /// <summary>
        /// Stops all running APIs
        /// </summary>
        public void StopApis() {
            //get a collection of all API Startup classes
            var ports = _runningApis.Keys.ToList();

            //iterate over all API Startup classes
            foreach (var port in ports) {
                //retrieve the API
                var api = _runningApis[port];
                //stop the API
                StopApi(port);
            }
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    StopApis(); //stop all APIs upon disposal of this class
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);
        }
        #endregion

    }
}
