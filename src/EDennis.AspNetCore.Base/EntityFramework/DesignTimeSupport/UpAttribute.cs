using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class UpAttribute : OnMethodBoundaryAspect {

        public bool CreateTestJsonTemporalTable { get; set; }
        public bool CreateSqlServerTemporalTables { get; set; }
        public bool SaveMappings { get; set; }
        public string PreSqlFolder { get; set; }
        public string PostSqlFolder { get; set; }
        public string TemporalInsertsFolder { get; set; }

        public UpAttribute(bool createTestJsonTemporalTable = true, 
            bool createSqlServerTemporalTables = true, 
            bool saveMappings = true, 
            string preSqlFolder = "MigrationsSql\\PreUp", 
            string postSqlFolder = "MigrationsSql\\PostUp",
            string temporalInsertsFolder = "MigrationsSql\\TemporalInserts") {
            CreateTestJsonTemporalTable = createTestJsonTemporalTable;
            CreateSqlServerTemporalTables = createSqlServerTemporalTables;
            SaveMappings = saveMappings;
            PreSqlFolder = preSqlFolder;
            PostSqlFolder = postSqlFolder;
            TemporalInsertsFolder = temporalInsertsFolder;
        }

        public override void OnEntry(MethodExecutionArgs args) {
            MigrationBuilder migrationBuilder = (MigrationBuilder)args.Arguments[0];

            migrationBuilder.CreateMaintenanceProcedures();

            if (CreateTestJsonTemporalTable)
                migrationBuilder.CreateTestJsonTableSupport();


            if(PreSqlFolder != null && Directory.Exists(PreSqlFolder)) {
                foreach (var file in Directory.GetFiles(PreSqlFolder).OrderBy(f=>f))
                    migrationBuilder.Sql(File.ReadAllText(file));
            }

        }

        public override void OnExit(MethodExecutionArgs args) {

            MigrationBuilder migrationBuilder = (MigrationBuilder)args.Arguments[0];

            if (CreateSqlServerTemporalTables)
                migrationBuilder.CreateSqlServerTemporalTables();

            if (SaveMappings)
                migrationBuilder.SaveMappings();

            if (PostSqlFolder != null && Directory.Exists(PostSqlFolder)) {
                foreach (var file in Directory.GetFiles(PostSqlFolder).OrderBy(f => f))
                    migrationBuilder.Sql(File.ReadAllText(file));
            }

            if (TemporalInsertsFolder != null && Directory.Exists(TemporalInsertsFolder)) {
                foreach (var file in Directory.GetFiles(TemporalInsertsFolder).OrderBy(f => f))
                    migrationBuilder.DoTemporalInserts(File.ReadAllText(file));
            }

        }
    }
}
