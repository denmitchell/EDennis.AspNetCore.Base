using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This class is used to hold data necessary
    /// to start an in-memory server for a given 
    /// web api assembly.
    /// 
    /// ASSUMPTION: All web api projects are in
    /// C:/{userprofile}/source/repos ...
    /// </summary>
    public class Api {

        // the path to ... source/repos
        readonly string _repoDir;

        /// <summary>
        /// Construct a new Api object and initialize
        /// the _repoDir variable, taking into consideration
        /// the user profile.
        /// </summary>
        public Api() {
            _repoDir = $"C:\\Users\\{Environment.UserName}\\source\\repos\\";
        }

        //a reference to an in-memory server
        public IWebHost Host { get; set; }

        //a reference to the process (when needed)
        public Process Process { get; set; }

        //the config key for the api
        public string ApiName { get; set; }

        //the name of the VS solution for the web api
        public string SolutionName { get; set; }

        //the name of the VS project for the web api
        public string ProjectName { get; set; }

        //the SDK version
        public decimal NetCoreAppVersion { get; set; }

        //the server's base address ({protocol}://{hostname}:{port}/)
        public string BaseAddress{ get; set; }

        //Holds all of the URLs for each controller associated with an api.
        //For microservices, this is often just a single controller/URL
        public Dictionary<string, string> ControllerUrls { get; set; }
            = new Dictionary<string, string>();

        //the fully qualified name of the Startup class
        public string StartupNamespaceAndClass { get => $"{ProjectName}.Startup"; }

        //the path to the project on the development computer
        public string LocalProjectDirectory { get => $"{_repoDir}{SolutionName}\\{ProjectName}"; }

        //the path to the DLL on the local computer
        public string AssemblyPath { get => $"{LocalProjectDirectory}\\bin\\Debug\\netcoreapp{NetCoreAppVersion.ToString("F1")}\\{ProjectName}.dll"; }
    }

}
