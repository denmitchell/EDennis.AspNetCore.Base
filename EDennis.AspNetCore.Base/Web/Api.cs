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

        //a reference to the process (when needed)
        public Process Process { get; set; }

        //the config key for the api
        public string ApiName { get; set; }

        //the name of the VS solution for the web api
        public string SolutionName { get; set; }

        //the name of the VS project for the web api
        public string ProjectName { get; set; }

        //the full path to the project.  This setting is
        //not needed if the default project path is used
        //c:\Users\{USER_PROFILE}\sources\repos\{SolutionName}\{ProjectName}
        public string FullProjectPath { get; set; }

        //launch profile to use (note: this will set the port)
        public string LaunchProfile { get; set; }

        //the server's port
        public int Port { get; set; }

        //Holds all of the URLs for each controller associated with an api.
        //For microservices, this is often just a single controller/URL
        public Dictionary<string, string> ControllerUrls { get; set; }
            = new Dictionary<string, string>();


        //the path to the project on the development computer
        public string LocalProjectDirectory {
            get {
                if (FullProjectPath == null)
                    return $"{_repoDir}{SolutionName}\\{ProjectName}";
                else
                    return FullProjectPath;
            }
        }

    }
}
