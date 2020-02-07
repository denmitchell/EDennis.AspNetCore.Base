using ConfigCore.Models;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib.Models {
    public class ConfigurationManager {

        public const string PROJECT_CONFIGURATIONS_FOLDER = "ProjectConfigurations";
        public const string SHARED_SETTINGS_FILE = "Shared.json";

        public async Task UploadNew() {

            Debug.WriteLine("Getting Project Configuration Files ...");
            var dir = Directory.EnumerateFiles(PROJECT_CONFIGURATIONS_FOLDER);

            Debug.WriteLine("Building Sql Connection/Command Objects ...");
            using var context = GetDbContext();
            var serverLastModifieds = await GetServerLastModifieds(context);
            using var cmd = GetSqlCommand(context);

            foreach (var filePath in dir) {
                var projectName = filePath.Substring(PROJECT_CONFIGURATIONS_FOLDER.Length + 1).Replace(".json", "");
                Debug.WriteLine($"Processing {projectName} ...");
                var lastModified = File.GetLastWriteTime(filePath);
                if(serverLastModifieds.TryGetValue(projectName,out DateTime serverLastModified)){
                    if (lastModified <= serverLastModified)
                        continue;
                }
                await Upload(context, PROJECT_CONFIGURATIONS_FOLDER, projectName);
            }
        }


        private async Task Upload(ConfigurationDbContext context, string folder, string projectName) {

            var config = new ConfigurationBuilder()
                .AddJsonFile($"{folder}/{SHARED_SETTINGS_FILE}", true)
                .AddJsonFile($"{folder}/{projectName}.json")
                .Build();

            List<ProjectSetting> projectSettings = 
                config.Flatten()
                .Select(x=>new ProjectSetting{
                    ProjectName = projectName,
                    SettingKey = x.Key, 
                    SettingValue = x.Value 
                }).ToList();


            Debug.WriteLine($"Uploading {projectName} ...");

            await context.BulkInsertOrUpdateOrDeleteAsync(projectSettings);

        }

        private SqlCommand GetSqlCommand(ConfigurationDbContext context) {
 
            SqlConnection cxn = (SqlConnection)context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            using SqlCommand cmd = new SqlCommand {
                Connection = cxn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SaveSettings"
            };

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = (SqlTransaction)trans.GetDbTransaction();

            cmd.Parameters.Add(
                new SqlParameter {
                    ParameterName = "@projectName",
                    SqlDbType = SqlDbType.VarChar
                });
            cmd.Parameters.Add(
                new SqlParameter { 
                    ParameterName = "@tvpSettings",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "dbo.SettingTableType"
                });
            return cmd;
        }

        private ConfigurationDbContext GetDbContext() {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true)
                .AddJsonFile($"appsettings.{env}.json", true)
                .Build();

            var cxnString = config["DbContexts:ConfigurationDbContext:ConnectionString"];

            var options = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlServer(cxnString)
                .Options;

            return new ConfigurationDbContext(options);
        }


        private async Task<Dictionary<string,DateTime>> GetServerLastModifieds(
            ConfigurationDbContext context) {

            Debug.WriteLine("Getting Server LastModified ...");

            SqlConnection cxn = (SqlConnection)context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            using SqlCommand cmd = new SqlCommand {
                Connection = cxn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetLastModified"
            };

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = (SqlTransaction)trans.GetDbTransaction();

            var dict = new Dictionary<string, DateTime>();

            DbDataReader reader = await cmd.ExecuteReaderAsync();

            if(reader.HasRows)
                while (reader.Read()){
                    dict.Add(reader.GetString(0), reader.GetDateTime(1));
                }

            return dict;
        }

    }

}
