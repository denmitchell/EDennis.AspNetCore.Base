using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This class is used to launch one or more
    /// Api dependencies.  The class can be used by
    /// an integration test or by a web application
    /// (e.g., for Swagger-oriented spot-testing).
    /// In the latter case, ApiLauncher should be
    /// dependency-injected as a singleton.  Note
    /// that the class depends upon the existence
    /// of an "Apis" section in the config
    /// (e.g., appsettings.Development.json) which
    /// has one or more named APIs (key), whose 
    /// properties include: SolutionName, ProjectName,
    /// NetCoreAppVersion, BaseAddress and 
    /// ControllerUrls.
    /// The development BaseAddress must be unique
    /// across all dependent APIs listed.
    /// </summary>
    public partial class ApiLauncher : IDisposable {

        //a dictionary of APIs that are running on an in-memory server 
        private Dictionary<Type, Api> _runningApis
            = new Dictionary<Type, Api>();

        //the configuration holding API data
        private IConfiguration _config;

        /// <summary>
        /// Constructs a new ApiLauncher with the
        /// provided configuration
        /// </summary>
        /// <param name="config">configuration holding API data</param>
        public ApiLauncher(IConfiguration config) {
            _config = config;

            StartApis(); //start all of the APIs
        }

        /// <summary>
        /// Retrieves data about the API, based
        /// upon the API name
        /// </summary>
        /// <param name="apiName">Name/Key for the API</param>
        /// <returns>an API object holding data</returns>
        public Api GetApi(string apiName) {
            return _runningApis
                .Where(a => a.Value.ApiName == apiName)
                .FirstOrDefault().Value;
        }

        /// <summary>
        /// Starts all APIs referenced in the configuration
        /// </summary>
        private void StartApis() {
            //get the API data from the configuration
            var apiSections = _config.GetSection("Apis").GetChildren();

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
                    NetCoreAppVersion = decimal.Parse(apiSection["NetCoreAppVersion"]),
                    BaseAddress = apiSection["BaseAddress"],
                    ControllerUrls = controllerUrls
                };

                //start the API
                StartApi(api);
            }
        }

        /// <summary>
        /// Stops all running APIs
        /// </summary>
        private void StopApis() {
            //get a collection of all API Startup classes
            var startupTypes = _runningApis.Keys.ToList();

            //iterate over all API Startup classes
            foreach (var type in startupTypes) {
                //retrieve the API
                var api = _runningApis[type];
                //stop the API
                StopApi(api, type);
            }
        }

        /// <summary>
        /// Starts the API in a new thread
        /// </summary>
        /// <param name="api">Api object</param>
        private void StartApi(Api api) {

            //load the relevant DLL
            var apiAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(api.AssemblyPath);
            //get a reference to the Startup class
            var startupType = apiAssembly.GetType(api.StartupNamespaceAndClass);

            //build a new IWebHost server, setting
            //the content root to the project's directory
            //and retrieving the appsettings config files
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseIISIntegration()
            .UseStartup(startupType)
            .UseContentRoot(api.LocalProjectDirectory)
            .ConfigureAppConfiguration(options => {
                options.SetBasePath(api.LocalProjectDirectory);
                options.AddJsonFile(api.LocalProjectDirectory + "\\appsettings.json");
                options.AddJsonFile(api.LocalProjectDirectory + "\\appsettings.Development.json");
            })
            .UseUrls(api.BaseAddress)
            .Build();

            //set a reference to the IWebHost server
            api.Host = host;

            //add the current API to the dictionary of running APIs
            _runningApis.Add(startupType, api);

            //within a separate thread, launch the IWebHost server
            Task.Run(() => {
                host.RunAsync();
            });

        }


        /// <summary>
        /// Stop a specific API
        /// </summary>
        /// <param name="api">The API object to stop</param>
        /// <param name="startupType">A reference to the Startup class</param>
        private void StopApi(Api api, Type startupType) {
            //stop the server
            if (api.Process != null) {
                api.Process.StandardInput.Close();
                api.Process.Close();
            } else {
                api.Host.StopAsync().Wait();
            }
            //remove the server from the dictionary of running servers
            _runningApis.Remove(startupType);
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
