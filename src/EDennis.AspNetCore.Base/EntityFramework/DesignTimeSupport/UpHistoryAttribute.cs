using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.EntityFrameworkCore.Migrations;
using EDennis.MigrationsExtensions;
using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public class UpHistoryAttribute : OnMethodBoundaryAspect {

        public bool SaveMappings { get; set; }
        public string PreSqlFolder { get; set; }
        public string PostSqlFolder { get; set; }

        public UpHistoryAttribute( 
            bool saveMappings = true, 
            string preSqlFolder = "MigrationsSql\\PreUp\\History", 
            string postSqlFolder = "MigrationsSql\\PostUp\\History") {
            SaveMappings = saveMappings;
            PreSqlFolder = preSqlFolder;
            PostSqlFolder = postSqlFolder;
        }

        public override void OnEntry(MethodExecutionArgs args) {
            MigrationBuilder migrationBuilder = (MigrationBuilder)args.Arguments[0];

            migrationBuilder.CreateMaintenanceProcedures();

            if(PreSqlFolder != null && Directory.Exists(PreSqlFolder)) {
                foreach (var file in Directory.GetFiles(PreSqlFolder).OrderBy(f => f))
                    migrationBuilder.Sql(File.ReadAllText(file));
            }

        }

        public override void OnExit(MethodExecutionArgs args) {

            MigrationBuilder migrationBuilder = (MigrationBuilder)args.Arguments[0];

            migrationBuilder.CreateMaintenanceProcedures();

            if (SaveMappings)
                migrationBuilder.SaveMappings();

            if (PostSqlFolder != null && Directory.Exists(PostSqlFolder)) {
                foreach (var file in Directory.GetFiles(PostSqlFolder).OrderBy(f => f))
                    migrationBuilder.Sql(File.ReadAllText(file));
            }

        }
    }
}
