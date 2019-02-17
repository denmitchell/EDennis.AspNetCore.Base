using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Build.Construction;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace EDennis.AspNetCore.Base.Testing {

    public class DatabaseCloneManager {


        public static CloneConnections GetCloneConnections(IConfiguration config, int cloneCount) {
            var contextsAndDatabases = GetProjectContextsAndDatabasesFromAppSettings(config);
            var connections = GetSharedConnections(contextsAndDatabases, cloneCount);
            var cloneConnections = new CloneConnections { CloneCount = cloneCount };
            foreach (var cxn in connections)
                cloneConnections.Add(cxn.Key, cxn.Value);
            return cloneConnections;
        }

        public static void PopulateCloneConnections(CloneConnections cloneConnections) {
            var contextsAndDatabases = GetSolutionContextsAndDatabasesFromAppSettings();
            var connections = GetSharedConnections(contextsAndDatabases, cloneConnections.CloneCount);
            foreach (var cxn in connections)
                cloneConnections.Add(cxn.Key, cxn.Value);
        }

        private static Dictionary<string, SqlConnectionAndTransaction[]> GetSharedConnections(
            Dictionary<string, string> contextsAndDatabases, int cloneCount) {
            var dict = new Dictionary<string, SqlConnectionAndTransaction[]>();
            var cxns = new SqlConnectionAndTransaction[cloneCount];
            var tasks = new List<Task>();
            foreach (var context in contextsAndDatabases.Keys) {
                dict.Add(context, cxns);
                for (int i = 0; i < cloneCount; i++) {
                    tasks.Add(AddConnection(dict, context, contextsAndDatabases[context], i));
                }
            }
            Task.WaitAll(tasks.ToArray());
            return dict;
        }

        public static void ReleaseClones(CloneConnections cloneConnections, int cloneIndex) {
            foreach(var context in cloneConnections.Keys) {
                if (cloneConnections != null && cloneConnections[context] != null 
                    && cloneConnections[context].Count() > cloneIndex) {
                    var connection = cloneConnections[context][cloneIndex];
                    ResetConnection(connection);
                }
            }
        }

        public static void ResetConnection(SqlConnectionAndTransaction cxn) {
            if (cxn.SqlConnection.State == ConnectionState.Open)
                cxn.SqlTransaction.Rollback();
            if (cxn.SqlConnection.State == ConnectionState.Closed)
                cxn.SqlConnection.Open();
            cxn.SqlConnection.ResetIdentities();
            cxn.SqlConnection.ResetSequences();
            cxn.SqlTransaction = cxn.SqlConnection.BeginTransaction(IsolationLevel.Serializable);
        }


        private static async Task AddConnection(
            Dictionary<string, SqlConnectionAndTransaction[]> dict,
            string context, string db, int cloneIndex) {
            await Task.Run(() => {
                var cxnString = $"Server=(localdb)\\ParallelDatabaseServer;Database={db}{cloneIndex};Trusted_Connection=True;MultipleActiveResultSets=true";
                var cxn = new SqlConnection(cxnString);
                cxn.Open();
                cxn.ResetIdentities();
                cxn.ResetSequences();
                var trans = cxn.BeginTransaction(IsolationLevel.Serializable);
                dict[context][cloneIndex] = new SqlConnectionAndTransaction 
                    { SqlConnection = cxn, SqlTransaction = trans };
            });
        }

        /// <summary>
        /// requires 
        ///     Microsoft.Build
        ///     Microsoft.Extensions.Configuration.Json
        ///     Microsoft.Extensions.Configuration.Binder
        ///     System.Data.SqlClient
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetSolutionContextsAndDatabasesFromAppSettings() {

            //get solution file path
            var solutionFile = GetSolutionfile();
            var dir = solutionFile.Directory;
            Directory.SetCurrentDirectory(dir.FullName);

            //get all appsettings.Development.json files
            //note: requires Microsoft.Build Nuget
            var appSettingsFiles = SolutionFile
                .Parse(solutionFile.FullName)
                .ProjectsInOrder
                .SelectMany(p => new DirectoryInfo(p.AbsolutePath.Replace($@"\{p.ProjectName}.csproj", "")).GetFiles("appsettings.Development.json"))
                .ToList();
            //note: I build this regex for sln files, which also works: (?:Project)(?:[^,]*,\s")(.*)(?:\\.*\.csproj)

            //initialize dictionary for all connection strings (optional return value)
            var contextsAndDatabases = new Dictionary<string, string>();

            //intialize list of databases
            var databases = new List<string>();

            foreach (var file in appSettingsFiles) {

                //build configs for each appsettings file
                var cxnStrings = new Dictionary<string, string>();
                var config = new ConfigurationBuilder()
                    .AddJsonFile(file.FullName)
                    .Build();
                //bind to internal dictionary
                config.GetSection("ConnectionStrings").Bind(cxnStrings);

                //add to main collection variables
                foreach (var c in cxnStrings) {
                    if (!contextsAndDatabases.ContainsKey(c.Key)) {
                        var cxnStrBuilder = new SqlConnectionStringBuilder {
                            ConnectionString = c.Value
                        };
                        contextsAndDatabases.Add(c.Key, cxnStrBuilder.InitialCatalog);
                    }
                }
            }
            //return result
            return contextsAndDatabases;
        }


        /// <summary>
        /// requires 
        ///     Microsoft.Extensions.Configuration.Json
        ///     Microsoft.Extensions.Configuration.Binder
        ///     System.Data.SqlClient
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetProjectContextsAndDatabasesFromAppSettings(IConfiguration config) {

            var cxnStrings = new Dictionary<string, string>();
            config.GetSection("ConnectionStrings").Bind(cxnStrings);

            //initialize dictionary for all connection strings (optional return value)
            var contextsAndDatabases = new Dictionary<string, string>();

            //intialize list of databases
            var databases = new List<string>();

            //add to main collection variables
            foreach (var c in cxnStrings) {
                if (!contextsAndDatabases.ContainsKey(c.Key)) {
                    var cxnStrBuilder = new SqlConnectionStringBuilder {
                        ConnectionString = c.Value
                    };
                    contextsAndDatabases.Add(c.Key, cxnStrBuilder.InitialCatalog);
                }
            }

            //return result
            return contextsAndDatabases;
        }



        /// <summary>
        /// Gets the solution file in a parent (or ancestor) directory
        /// </summary>
        /// <param name="currentPath">optional current path</param>
        /// <returns></returns>
        ///from https://stackoverflow.com/a/35824406/10896865
        private static FileInfo GetSolutionfile(string currentPath = null) {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any()) {
                directory = directory.Parent;
            }
            return directory.GetFiles("*.sln").FirstOrDefault();
        }

    }
}
