using ConfigCore.Models;
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
                Debug.WriteLine($"Processing {filePath} ...");
                var projectName = filePath;
                var lastModified = File.GetLastWriteTime(filePath);
                if(serverLastModifieds.TryGetValue(projectName,out DateTime serverLastModified)){
                    if (lastModified <= serverLastModified)
                        continue;
                }
                await Upload(cmd, PROJECT_CONFIGURATIONS_FOLDER, projectName);
            }
        }


        private async Task Upload(SqlCommand cmd, string folder, string projectName) {

            var config = new ConfigurationBuilder()
                .AddJsonFile($"{folder}/{SHARED_SETTINGS_FILE}", true)
                .AddJsonFile($"{folder}/{projectName}")
                .Build();

            IEnumerable<ConfigSetting> configSettings = 
                config.Flatten()
                .Select(x=>new ConfigSetting{ 
                    SettingKey = x.Key, 
                    SettingValue = x.Value 
                });

            cmd.Parameters["@projectName"].Value = projectName;
            cmd.Parameters["@tvpSettings"].Value = configSettings;

            Debug.WriteLine($"Uploading {projectName} ...");
            await cmd.ExecuteNonQueryAsync();

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
