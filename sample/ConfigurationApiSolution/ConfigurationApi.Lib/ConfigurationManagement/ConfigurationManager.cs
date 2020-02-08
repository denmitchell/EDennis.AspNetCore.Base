using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib.Models {
    public class ConfigurationManager {

        public const string PROJECT_CONFIGURATIONS_FOLDER = "ProjectConfigurations";
        public const string SHARED_SETTINGS_FOLDER = "Shared";

        public async Task UploadNew() {

            Debug.WriteLine("Getting Project Configuration Files ...");
            var folders = Directory.EnumerateDirectories(PROJECT_CONFIGURATIONS_FOLDER);

            Debug.WriteLine("Building Sql Connection/Command Objects ...");
            var context = GetDbContext();
            var serverLastModifieds =
                context.ProjectSettings.GroupBy(a => a.ProjectName)
                .Select(g => new {
                    ProjectName = g.Key,
                    MaxSysStart = g.Max(x => x.SysStart)
                }).ToDictionary(x => x.ProjectName, x => x.MaxSysStart);
                
            //using var cmd = GetSqlCommand(context);

            foreach (var folder in folders.Where(f=>!f.EndsWith(SHARED_SETTINGS_FOLDER))) {
                var projectName = folder.Substring(PROJECT_CONFIGURATIONS_FOLDER.Length + 1);
                Debug.WriteLine($"Processing {projectName} ...");
                var lastModified = File.GetLastWriteTime(folder);
                if(serverLastModifieds.TryGetValue(projectName,out DateTime serverLastModified)){
                    if (lastModified <= serverLastModified)
                        continue;
                }
                Upload(context, folder, projectName);
            }
            await context.SaveChangesAsync();

        }


        private void Upload(ConfigurationDbContext context, string folder, string projectName) {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .AddJsonFile($"{PROJECT_CONFIGURATIONS_FOLDER}\\{SHARED_SETTINGS_FOLDER}\\appsettings.json", true)
                .AddJsonFile($"{PROJECT_CONFIGURATIONS_FOLDER}\\{SHARED_SETTINGS_FOLDER}\\appsettings.{env}.json", true)
                .AddJsonFile($"{folder}\\appsettings.json", true)
                .AddJsonFile($"{folder}\\appsettings.{env}.json", true)
                .Build();

            List<ProjectSetting> projectSettings = 
                config.Flatten()
                .Select(x=>new ProjectSetting{
                    ProjectName = projectName,
                    SettingKey = x.Key, 
                    SettingValue = x.Value 
                }).ToList();


            Debug.WriteLine($"Uploading {projectName} ...");

            var existingProjectSettings = context.ProjectSettings.Where(ps => ps.ProjectName == projectName);
            foreach(var projectSetting in existingProjectSettings) {
                context.ProjectSettings.Remove(projectSetting);
            }
            foreach (var projectSetting in projectSettings) {
                context.ProjectSettings.Add(projectSetting);
            }
        }


        private ConfigurationDbContext GetDbContext() {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(ConfigurationApiConfiguration.GetConfiguration(env))
                .Build();

            var cxnString = config["DbContexts:ConfigurationDbContext:ConnectionString"];

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(cxnString)
                .Options;

            return new ConfigurationDbContext(options);
        }


    }

}
